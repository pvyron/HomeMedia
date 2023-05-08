using HomeMedia.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddTorrents(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello world");

app.MapGet("/test", () => "Working");

//app.MapPost("/api/torrents/search", async ([FromServices] ITorrentSearchService torrentSearchService, [FromBody] TorrentsSearchRequestModel requestModel) =>
//{
//    var infos = await torrentSearchService.QueryTorrentDataAsync(new HomeMedia.Application.Torrents.Models.TorrentSearchParams
//    {
//        Name = requestModel.Query
//    }, CancellationToken.None).ToListAsync();

//    return infos.Select(info => new TorrentsSearchResponseModel
//    {
//        Category = info.Category,
//        Download = info.MagnetLink,
//        Filename = info.Name,
//        Seeders = info.Seeders,
//        Size = info.Size,
//        SizeText = info.Size //((long)info.Size).GetBytesReadable()
//    });
//});

Console.WriteLine("Before build");

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}