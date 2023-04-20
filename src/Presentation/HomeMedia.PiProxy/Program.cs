using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Contracts.Torrents;
using HomeMedia.Infrastructure;
using HomeMedia.Infrastructure.Tools;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddTorrents();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello world");

app.MapGet("/test", () => "Working");

app.MapPost("/api/torrents/search", async ([FromServices] ITorrentSearchService torrentSearchService, [FromBody] TorrentsSearchRequestModel requestModel) =>
{
    var infos = await torrentSearchService.QueryTorrentDataAsync(new HomeMedia.Application.Torrents.Models.TorrentSearchParams
    {
        Name = requestModel.Query
    });

    return infos.Select(info => new TorrentsSearchResponseModel
    {
        Category = info.Category,
        Download = info.Download,
        Filename = info.Filename,
        Seeders = info.Seeders,
        Size = info.Size,
        SizeText = info.Size //((long)info.Size).GetBytesReadable()
    });
});

Console.WriteLine("Before build");

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}