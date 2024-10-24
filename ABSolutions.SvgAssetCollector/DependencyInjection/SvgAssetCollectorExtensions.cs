using ABSolutions.SvgAssetCollector.Models;
using ABSolutions.SvgAssetCollector.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ABSolutions.SvgAssetCollector.DependencyInjection;

public static class SvgAssetCollectorExtensions
{
    public static IServiceCollection AddSvgAssetCollector(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<SvgAssetCollectorConfiguration>(
            configuration.GetSection(SvgAssetCollectorConfiguration.AppSettingsKey));
        services.AddSingleton<ISvgCache, SvgCacheInMemory>();
        services.AddSingleton<ISvgAssetCollector, Services.SvgAssetCollector>();
        return services;
    }
}