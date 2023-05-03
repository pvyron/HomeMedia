using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Infrastructure.Torrents.Options;
using HomeMedia.Infrastructure.Torrents.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeMedia.Infrastructure;
public static class ServiceInstallers
{
    public static IServiceCollection AddTorrents(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<VHomeTorrentSearchServiceOptions>().Bind(configuration.GetSection(VHomeTorrentSearchServiceOptions.SectionName));

        services.AddSingleton<ITorrentSearchService, VHomeTorrentSearchService>();
        services.AddSingleton<ITorrentClientService, TorrentClientService>();

        return services;
    }
}
