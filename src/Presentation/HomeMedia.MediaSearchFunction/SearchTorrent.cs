using System.Net;
using System.Text.Json;
using Azure.Core.Serialization;
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
    private readonly JsonSerializerOptions _serializer = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false,
    };

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
            var requestModel = await JsonSerializer.DeserializeAsync<TorrentsSearchRequestModel>(req.Body, _serializer);

            if (requestModel is null)
            {
                _logger.LogInformation("{Request} came out null", await req.ReadAsStringAsync());

                return req.CreateResponse(HttpStatusCode.NoContent);
            }

            var torrents = await _torrentSearchService.QueryTorrentDataAsync(new Application.Torrents.Models.TorrentSearchParams { QueryString = requestModel.Name }, CancellationToken.None).ToListAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(torrents.Select(t => new TorrentsSearchResponseModel
            {
                Category = t.Category,
                Download = t.MagnetLink,
                Filename = t.Name,
                Seeders = t.Seeders,
                Size = t.Size,
                SizeText = t.Size
            }), new JsonObjectSerializer(_serializer));

            return response;
        }
        catch (Exception ex)
        {
            var response = req.CreateResponse(HttpStatusCode.BadRequest);

            await response.WriteStringAsync(ex.Message);

            return response;
        }
    }
}
