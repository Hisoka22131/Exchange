using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Exchange.Web.Endpoints;

public static class EndpointsExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var serviceDescriptors = typeof(EndpointForAssembly)
            .Assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }
    
    public static void MapEndpoints(this IApplicationBuilder app)
    {
        var endpoints = app.ApplicationServices.GetServices<IEndpoint>();

        var endpointRouteBuilder = app as IEndpointRouteBuilder
                                   ?? throw new InvalidOperationException("Application builder is not an IEndpointRouteBuilder.");

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(endpointRouteBuilder);
        }
    }
}