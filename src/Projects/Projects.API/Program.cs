using Designly.Auth;
using Designly.Auth.Extentions;
using Designly.Auth.Identity;
using Designly.Configuration;
using Designly.Shared;
using Designly.Shared.Extensions;
using Designly.Shared.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using Projects.Application;
using Projects.Application.Features.CreateProject;
using Projects.Application.Features.DeleteProject;
using Serilog;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
var configuration = builder.Configuration;

// Configure Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()  // TODO: Read from configuration
    .MinimumLevel.Debug()
    );

// API versioning
ConfigureVersioning(builder);

// Enabled authentication
builder.Services.AddJwtBearerConfig(configuration);

RegisterAuthorizationAndPolicyHandlers(builder);

// Configure Swagger
builder.Services.ConfigureSecuredSwagger("projects", "v1");
builder.Services.ConfigureCors();

// Wire up the exception handlers
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

// Configure Services
builder.Services.AddHttpClient("cognito", client =>
{
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaTypeNames.Application.Json);
    client.BaseAddress = new Uri("https://designflow.auth.us-east-1.amazoncognito.com/oauth2/token");
});
builder.Services.AddApplication(configuration);
// get AccountsApiConfiguration from configuration and configure it
builder.Services.Configure<AccountsApiConfiguration>(configuration.GetSection(nameof(AccountsApiConfiguration)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

MapEndoints(app);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<TenantProviderMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void RegisterAuthorizationAndPolicyHandlers(WebApplicationBuilder builder)
{
    builder.Services.AddAuthorizationBuilder()
        .AddPolicy(IdentityData.AdminUserPolicyName, policyBuilder => policyBuilder.AddRequirements(new MustBeAdminRequirement()))
        .AddPolicy(IdentityData.AccountOwnerPolicyName, policyBuilder => policyBuilder.AddRequirements(new MustBeAccountOwnerRequirement()));

    builder.Services.AddSingleton<IAuthorizationHandler, MustBeAdminRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, MustBeAccountOwnerRequirementHandler>();
}

static void MapEndoints(WebApplication app)
{
    var versionSet = app.NewApiVersionSet()
        .HasApiVersion(new Asp.Versioning.ApiVersion(1))
        .ReportApiVersions()
        .Build();

    var routeGroup = app
        .MapGroup("api/v{version:apiVersion}")
        .RequireAuthorization()
        .WithApiVersionSet(versionSet);

    routeGroup.MapCreateFeature();
    routeGroup.MapDeleteFeature("{projectId}");
}

static void ConfigureVersioning(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = Asp.Versioning.ApiVersionReader.Combine(
            new Asp.Versioning.UrlSegmentApiVersionReader(),
            new Asp.Versioning.HeaderApiVersionReader(Designly.Shared.Consts.ApiVersionHeaderEntry));
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
    });
}