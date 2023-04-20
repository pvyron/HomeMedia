using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Infrastructure.Torrents.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HomeMedia.Infrastructure;
public static class ServiceInstallers
{
    public static IServiceCollection AddTorrents(this IServiceCollection services)
    {
        services.AddSingleton<ITorrentSearchService, RyukTorrentSearchService>();
        services.AddSingleton<ITorrentClientService, TorrentClientService>();

        return services;
    }
}
