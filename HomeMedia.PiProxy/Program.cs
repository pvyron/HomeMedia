using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Contracts.Torrents;
using HomeMedia.Infrastructure;
using HomeMedia.Infrastructure.Tools;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddTorrents();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/index", _ => Task.FromResult("Hello world"));

app.MapPost("/api/torrents/search", async ([FromServices]ITorrentSearchService torrentSearchService, [FromBody] TorrentsSearchRequestModel requestModel) =>
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
        SizeText = ((long)info.Size).GetBytesReadable()
    });
});

await app.RunAsync();