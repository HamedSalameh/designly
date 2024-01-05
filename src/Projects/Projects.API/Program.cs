using Designly.Shared.Identity;
using Microsoft.AspNetCore.Authorization;
using Projects.Application;
using Projects.Application.Extentions;
using Projects.Application.Features.CreateProject;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
var configuration = builder.Configuration;

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
        new Asp.Versioning.HeaderApiVersionReader("api-version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
});

// Enabled authentication
builder.Services.AddJwtBearerConfig(configuration);

RegisterAuthorizationAndPolicyHandlers(builder);

// Configure Swagger
builder.Services.ConfigureSecuredSwagger();
builder.Services.ConfigureCors();

// Configure Services
builder.Services.AddApplication(configuration);
builder.Services.AddSingleton<IAuthorizationProvider, AuthorizationProvider>();

// Configure Health checks
builder.Services.AddHealthChecks();

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new Asp.Versioning.ApiVersion(1))
    .ReportApiVersions()
    .Build();

var routeGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .RequireAuthorization()
    .WithApiVersionSet(versionSet);

routeGroup.MapCreateFeature("/hello");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void RegisterAuthorizationAndPolicyHandlers(WebApplicationBuilder builder)
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(IdentityData.AdminUserPolicyName,
            policyBuilder => policyBuilder.AddRequirements(new MustBeAdminRequirement()));

        options.AddPolicy(IdentityData.AccountOwnerPolicyName,
            policyBuilder => policyBuilder.AddRequirements(new MustBeAccountOwnerRequirement()));
    });

    builder.Services.AddSingleton<IAuthorizationHandler, MustBeAdminRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, MustBeAccountOwnerRequirementHandler>();
}