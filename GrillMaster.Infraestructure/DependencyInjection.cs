using GrillMaster.Domain.Abstractions;
using GrillMaster.Domain.DTOs;
using GrillMaster.Infrastructure.ExternalApi;
using GrillMaster.Infrastructure.Packing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace GrillMaster.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string menuApiBaseUrl)
    {
        services.AddSingleton<IBinPackingService, GuillotinePacker>();

        services.AddHttpClient<IMenuApiClient, MenuApiClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(menuApiBaseUrl);
            });

        return services;
    }
}