using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHttpClient();
        services.AddTorrents();
    })
    .Build();

var service = host.Services.GetService<ITorrentSearchService>();

var query = "";

var torrentData = await service!.QueryTorrentDataAsync(query);

await host.StopAsync();