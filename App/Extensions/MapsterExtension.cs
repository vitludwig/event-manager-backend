using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using EventApp.App.API.V1;
using EventApp.App.Dal;

namespace EventApp.App.Extensions;

public static class MapsterExtension
{
    public static void AddMapster(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(V1).Assembly);
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
        
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();
    }
}