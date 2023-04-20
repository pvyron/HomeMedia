using HomeMedia.Application.Torrents.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HomeMedia.Infrastructure;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((builder, services) =>
    {
        services.AddHttpClient();
        services.AddTorrents();
    })
    .Build();

host.Run();
