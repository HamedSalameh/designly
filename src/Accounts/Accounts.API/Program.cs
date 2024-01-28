using Projects.Application;
using Designly.Auth.Identity;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Accounts.Application.Features.CreateAccount;
using Designly.Auth.Extentions;
using Designly.Shared.Extensions;
using Accounts.API.Extensions;
using Designly.Shared.Middleware;
using Accounts.Application.Features.ValidateUser;
using Designly.Auth;
using Designly.Auth.Policies;

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
builder.Services.ConfigureSecuredSwagger("accounts", "v1");
builder.Services.ConfigureCors();

// Wire up the exception handlers
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessLogicExceptionHandler>();
builder.Services.AddExceptionHandler<AccountExceptionsHandler>();
builder.Services.AddProblemDetails();

// Configure Services
builder.Services.AddHttpClient();
builder.Services.AddApplication(configuration);

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

app.Run();

static void RegisterAuthorizationAndPolicyHandlers(WebApplicationBuilder builder)
{
    builder.Services.AddAuthorizationBuilder()
        .AddPolicy(IdentityData.AdminUserPolicyName, policyBuilder => policyBuilder.AddRequirements(new MustBeAdminRequirement()))
        .AddPolicy(IdentityData.AccountOwnerPolicyName, policyBuilder => policyBuilder.AddRequirements(new MustBeAccountOwnerRequirement()))
        .AddPolicy(IdentityData.ServiceAccountPolicyName, policyBuilder => policyBuilder.AddRequirements(new MustBeServiceAccountRequirement()));

    builder.Services.AddSingleton<IAuthorizationHandler, MustBeAdminRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, MustBeAccountOwnerRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, ServiceAccountAuthorizationHandler>();
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
    routeGroup.MapValidateUserFeature();
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