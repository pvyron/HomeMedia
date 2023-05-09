using HomeMedia.Infrastructure;
using HomeMedia.PiProxy;
using HomeMedia.PiProxy.Torrents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddJsonOptions();
builder.Services.AddTorrents(builder.Configuration);
builder.Services.AddHostedService<TorrentDownloadWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/", () => "I'm Working!");

Console.WriteLine("Before build");

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}