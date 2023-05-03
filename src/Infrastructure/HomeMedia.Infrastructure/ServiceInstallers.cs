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

        services.Configure<ExternalTorrentQueueServiceOptions>(options =>
        {
            options.ConnectionString = configuration.GetConnectionString("ExternalTorrentQueue")!;
            options.QueueName = configuration.GetValue<string>("ExternalTorrentQueue:Name")!;
        });

        services.AddSingleton<ITorrentSearchService, VHomeTorrentSearchService>();
        services.AddSingleton<IExternalTorrentQueueService, ExternalTorrentQueueService>();

        return services;
    }
}
