using HomeMedia.Application;
using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Infrastructure.Torrents.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure;
public static class ServiceInstallers
{
    public static IServiceCollection AddTorrents(this IServiceCollection services)
    {
        services.AddSingleton<ITorrentSearchService, TorrentSearchService>();
        services.AddSingleton<ITorrentClientService, TorrentClientService>();

        return services;
    }
}
