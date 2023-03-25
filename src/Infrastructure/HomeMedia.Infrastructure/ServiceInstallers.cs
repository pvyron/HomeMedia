using HomeMedia.Application;
using HomeMedia.Application.Torrents.Interfaces;
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

        return services;
    }
}
