using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Emata.Exercise.LoansManagement.Shared.Endpoints;

public static class EndpointConfigurationExtensions
{
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoints)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoints), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoints> endpoints = app.Services
            .GetRequiredService<IEnumerable<IEndpoints>>();

        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoints endpoint in endpoints)
        {
            if (endpoint.Prefix is not null && routeGroupBuilder is null)
            {
                //this is a minimal API endpoint with a prefix defined but no route group builder provided
                //so we need to create a route group for it
                var group = app.MapGroup(endpoint.Prefix)
                    .WithTags(endpoint.Prefix);
                endpoint.MapEndpoints(group);
            }
            else
            {
                endpoint.MapEndpoints(builder);
            }
        }

        return app;
    }
}