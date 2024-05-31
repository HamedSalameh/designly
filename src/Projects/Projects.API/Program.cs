using Designly.Auth;
using Designly.Auth.Extentions;
using Designly.Auth.Models;
using Designly.Base.Exceptions;
using Designly.Configuration;
using Designly.Shared.Extensions;
using Designly.Shared.Middleware;
using Microsoft.Net.Http.Headers;
using Projects.Application;
using Projects.Application.Features.CreateProject;
using Projects.Application.Features.CreateTask;
using Projects.Application.Features.DeleteProject;
using Projects.Application.Features.DeleteTask;
using Projects.Application.Features.SearchProjects;
using Projects.Application.Features.SearchTasks;
using Projects.Application.Features.UpdateProject;
using Projects.Application.Features.UpdateTask;
using Serilog;
using System.Net.Http.Headers;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
var configuration = builder.Configuration;

// Configure Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(configuration));

// API versioning
ConfigureVersioning(builder);

// Enabled authentication
builder.Services.AddJwtBearerConfig(configuration);

builder.Services.RegisterAuthorizationAndPolicyHandlers();

// Configure Swagger
builder.Services.ConfigureSecuredSwagger("projects", "v1");
builder.Services.ConfigureCors();

// Wire up the exception handlers
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessLogicExceptionHandler>();
builder.Services.AddProblemDetails();

// Configure Services
builder.Services.AddHttpClient();

// Configure each service separately using IOptions
builder.Services.Configure<AccountsServiceConfiguration>(configuration.GetSection(AccountsServiceConfiguration.Position));
builder.Services.Configure<ClientsServiceConfiguration>(configuration.GetSection(ClientsServiceConfiguration.Position));

AttachNamedHttpClient<AccountsServiceConfiguration>(builder, AccountsServiceConfiguration.Position);
AttachNamedHttpClient<ClientsServiceConfiguration>(builder, ClientsServiceConfiguration.Position);

builder.Services.Configure<OAuth2ServiceProviderConfiguration>(configuration.GetSection(nameof(OAuth2ServiceProviderConfiguration)));

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

app.MapControllers();

app.Run();


static void MapEndoints(WebApplication app)
{
    var versionSet = app.NewApiVersionSet()
        .HasApiVersion(new Asp.Versioning.ApiVersion(1))
        .ReportApiVersions()
        .Build();

    var projectsRouteGroup = app
        .MapGroup("api/v{version:apiVersion}/projects")
        .RequireAuthorization()
        .WithApiVersionSet(versionSet);

    projectsRouteGroup.MapCreateFeature();
    projectsRouteGroup.MapDeleteFeature("{projectId}");
    projectsRouteGroup.MapUpdateFeature("{projectId}");
    projectsRouteGroup.MapSearchFeature("search");

    projectsRouteGroup.MapCreateTaskFeature("tasks");
    projectsRouteGroup.MapDeleteTaskFeature("{projectId}/tasks/{taskId}");
    projectsRouteGroup.MapUpdateTaskFeature("{projectId}/tasks/{taskId}");
    projectsRouteGroup.MapSearchTasksEndpoint("tasks/search");
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

static void AttachNamedHttpClient<T>(WebApplicationBuilder builder, string section) where T : ServiceConfiguration
{
    var accountsServiceConfig = builder.Configuration.GetSection(section).Get<T>() ?? throw new ConfigurationException($"Could not find configuration for {section}");
    if (string.IsNullOrEmpty(accountsServiceConfig.ServiceName))
    {
        throw new ConfigurationException(nameof(accountsServiceConfig.ServiceName));
    }

    var clientName = accountsServiceConfig.ServiceName;
    var baseAddress = accountsServiceConfig.BaseUrl;
    var serviceUri = accountsServiceConfig.ServiceUrl;
    var apiVersion = accountsServiceConfig.ApiVersion;

    builder.Services.AddHttpClient(clientName, client =>
    {
        client.BaseAddress = new Uri($"{baseAddress}/{serviceUri}");
        client.DefaultRequestHeaders.Add(nameof(HeaderNames.Accept), MediaTypeNames.Application.Json);
        client.DefaultRequestHeaders.Add(Designly.Shared.Consts.ApiVersionHeaderEntry, apiVersion);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
    });
}
