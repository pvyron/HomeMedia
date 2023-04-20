using System.Net;
using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Contracts.Torrents;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace HomeMedia.MediaSearchFunction;

public class SearchTorrent
{
    private readonly ILogger _logger;
    private readonly ITorrentSearchService _torrentSearchService;

    public SearchTorrent(ILoggerFactory loggerFactory, ITorrentSearchService torrentSearchService)
    {
        _logger = loggerFactory.CreateLogger<SearchTorrent>();
        _torrentSearchService = torrentSearchService;
    }

    [Function("SearchMedia")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        try
        {
            var requestModel = await System.Text.Json.JsonSerializer.DeserializeAsync<TorrentsSearchRequestModel>(req.Body);

            if (requestModel is null)
            {
                _logger.LogInformation("{Request} came out null", await req.ReadAsStringAsync());

                return req.CreateResponse(HttpStatusCode.NoContent);
            }

            var torrents = await _torrentSearchService.QueryTorrentDataAsync(new Application.Torrents.Models.TorrentSearchParams { Name = requestModel.Query });

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(torrents.Select(t => new TorrentsSearchResponseModel
            {
                Category = t.Category,
                Download = t.Download,
                Filename = t.Filename,
                Seeders = t.Seeders,
                Size = t.Size,
                SizeText = t.Size
            }));

            return response;
        }
        catch (Exception ex)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
