using HomeMedia.Application.Torrents.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HomeMedia.Infrastructure;
using Microsoft.Extensions.Configuration;

var host = new HostBuilder()
    .ConfigureHostConfiguration(builder =>
    {
        builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
        builder.AddEnvironmentVariables();
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((builder, services) =>
    {
        services.AddHttpClient();
        services.AddTorrents(builder.Configuration);
    })
    .Build();

host.Run();
