using Asp.Versioning.Builder;
using Projects.Application.Features.CreateOrUpdateProperty;
using Projects.Application.Features.CreateProject;
using Projects.Application.Features.CreateTask;
using Projects.Application.Features.DeleteProject;
using Projects.Application.Features.DeleteProperty;
using Projects.Application.Features.DeleteTask;
using Projects.Application.Features.SearchProjects;
using Projects.Application.Features.SearchTasks;
using Projects.Application.Features.UpdateProject;
using Projects.Application.Features.UpdateTask;

public static class EndpointsExtentions
{
    public static void MapEndoints(WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new Asp.Versioning.ApiVersion(1))
            .ReportApiVersions()
            .Build();
        
        CreateProjectsFeatures(app, versionSet);

        CreateRealEstatePropertiesFeatures(app, versionSet);
    }

    private static void CreateProjectsFeatures(WebApplication app, ApiVersionSet versionSet)
    {
        var projectsRouteGroup = app
                    .MapGroup("api/v{version:apiVersion}/projects")
                    .RequireAuthorization()
                    .WithApiVersionSet(versionSet);

        projectsRouteGroup.MapCreateFeature();
        projectsRouteGroup.MapDeleteFeature("{projectId}");
        projectsRouteGroup.MapUpdateFeature("{projectId}");
        projectsRouteGroup.MapSearchFeature("search");

        projectsRouteGroup.MapCreateTaskFeature("{projectId}/tasks");
        projectsRouteGroup.MapDeleteTaskFeature("{projectId}/tasks/{taskId}");
        projectsRouteGroup.MapUpdateTaskFeature("{projectId}/tasks/{taskId}");
        projectsRouteGroup.MapSearchTasksEndpoint("{projectId}/tasks/search");
    }

    private static void CreateRealEstatePropertiesFeatures(WebApplication app, ApiVersionSet versionSet)
    {
        var realEstatePropertyRouteGroup = app
                    .MapGroup("api/v{version:apiVersion}/realestate")
                    .RequireAuthorization()
                    .WithApiVersionSet(versionSet);
        realEstatePropertyRouteGroup.MapCreateOrUpdatePropertyEndpoint("properties");
        realEstatePropertyRouteGroup.MapDeletePropertyFeature("properties/{propertyId}");
    }
}