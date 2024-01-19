using Projects.Application;
using Designly.Auth.Identity;
using Designly.Shared;
using Designly.Shared.Extentions;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Designly.Auth.Providers;
using Accounts.Application.Features.CreateAccount;
using Designly.Auth.Extentions;

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = Asp.Versioning.ApiVersionReader.Combine(
        new Asp.Versioning.UrlSegmentApiVersionReader(),
        new Asp.Versioning.HeaderApiVersionReader(Consts.ApiVersionHeaderEntry));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
});

// Enabled authentication
builder.Services.AddJwtBearerConfig(configuration);

RegisterAuthorizationAndPolicyHandlers(builder);

// Configure Swagger
builder.Services.ConfigureSecuredSwagger("accounts", "v1");
builder.Services.ConfigureCors();

// Configure Services
builder.Services.AddHttpClient();
builder.Services.AddApplication(configuration);
builder.Services.AddSingleton<IAuthorizationProvider, AuthorizationProvider>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

MapEndoints(app);

app.UseHttpsRedirection();

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

    routeGroup.MapCreateAccountFeature();
}