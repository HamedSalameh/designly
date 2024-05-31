using Projects.Application;
using Serilog;
using Accounts.Application.Features.CreateAccount;
using Designly.Auth.Extentions;
using Designly.Shared.Extensions;
using Accounts.API.Extensions;
using Designly.Shared.Middleware;
using Accounts.Application.Features.ValidateUser;
using Designly.Auth;

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