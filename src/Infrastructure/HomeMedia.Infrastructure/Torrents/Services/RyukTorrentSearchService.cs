using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Application.Torrents.Models;
using HomeMedia.Infrastructure.Torrents.Dto;
using HomeMedia.Models.Torrents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HomeMedia.Infrastructure.Torrents.Services;
internal sealed class RyukTorrentSearchService : ITorrentSearchService
{
    private readonly string _apiUrl;
    private readonly ILogger<RyukTorrentSearchService> _logger;
    private readonly HttpClient _httpClient;

    public RyukTorrentSearchService(ILogger<RyukTorrentSearchService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _apiUrl = configuration.GetRequiredSection("TorrentSearch:ApiUrl")!.Value!;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<List<TorrentInfo>> QueryTorrentDataAsync(TorrentSearchParams torrentSearchParams, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Request {search}", torrentSearchParams);

        var response = await _httpClient.GetAsync($"{_apiUrl}{torrentSearchParams.AsQueryStringRyuk()}");

        var responseContent = await response.Content.ReadAsStringAsync();

        try
        {
            var searchResponse = JsonSerializer.Deserialize<List<TorrentSearchRyukResponseModel>>(responseContent);

            if (searchResponse is null)
            {
                throw new HttpRequestException($"Request was unsuccessful");
            }

            return searchResponse?.Select(r =>
            new TorrentInfo
            {
                Category = r.Category ?? "",
                MagnetLink = r.Magnet ?? "",
                Name = r.Name ?? "",
                Seeders = r.Seeders ?? "0",
                Size = r.Size ?? "",
            }).ToList() ?? new List<TorrentInfo>();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Exception} was thrown after request to {url} with {search params}", ex, _apiUrl, torrentSearchParams);
            throw;
        }
    }

    async IAsyncEnumerable<TorrentInfo> ITorrentSearchService.QueryTorrentDataAsync(TorrentSearchParams torrentSearchParams, [EnumeratorCancellation] CancellationToken cancellationToken)
    {

        await Task.CompletedTask;
        yield break;
        throw new NotImplementedException();
    }

    readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        MaxDepth = 10,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = true,
        DefaultBufferSize = 128
    };
}
