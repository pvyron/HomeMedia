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

Console.WriteLine("Give torrent title");
string? name = null;

while (string.IsNullOrWhiteSpace(name))
{
    name = Console.ReadLine();
}

var torrentData = await service!.QueryTorrentDataAsync(new HomeMedia.Application.Torrents.Models.TorrentSearchParams
{
    Name = name
});

foreach (var torrent in torrentData)
{
    Console.WriteLine();
    Console.WriteLine("------------------------------------------------------------------");
    Console.WriteLine();
    Console.WriteLine(torrent.Filename);
    Console.WriteLine(torrent.Download);
}

Console.ReadLine();

await host.StopAsync();