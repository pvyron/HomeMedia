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

var searchService = host.Services.GetService<ITorrentSearchService>();
var clientService = host.Services.GetService<ITorrentClientService>();

while (true)
{
    Console.WriteLine("Give torrent title");
    string? name = null;

    while (string.IsNullOrWhiteSpace(name))
    {
        name = Console.ReadLine();
    }

    var torrentData = await searchService!.QueryTorrentDataAsync(new HomeMedia.Application.Torrents.Models.TorrentSearchParams
    {
        Name = name
    });

    foreach (var torrent in torrentData.ToList().OrderByDescending(t => t.Seeders))
    {
        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------------");
        Console.WriteLine();
        Console.WriteLine(torrent.Filename);
        Console.WriteLine(torrent.Download);
        Console.WriteLine("Size: " + torrent.Size);
        Console.WriteLine("Seeders: " + torrent.Seeders);
    }

    if (torrentData.Any())
    {
        //await clientService!.DownloadTorrent(torrentData.First().Download, "D:\\Media");
    }

}

Console.ReadLine();

await host.StopAsync();