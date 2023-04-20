using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Application.Torrents.Models;
using HomeMedia.Infrastructure.Torrents.Dto;
using HomeMedia.Infrastructure.Torrents.Models;
using HomeMedia.Models.Torrents;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HomeMedia.Infrastructure.Torrents.Services;
internal sealed class RarbgTorrentSearchService : ITorrentSearchService
{
    private readonly string _apiUrl = "https://torrentapi.org/pubapi_v2.php?app_id=vyron_torrent_app";

    private readonly ILogger<RarbgTorrentSearchService> _logger;
    private readonly HttpClient _client;
    private TorrentsApiAccessToken? _accessToken;

    public RarbgTorrentSearchService(ILogger<RarbgTorrentSearchService> logger, IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public async Task<List<TorrentInfo>> QueryTorrentDataAsync(TorrentSearchParams torrentSearchParams)
    {
        _logger.LogInformation("Request {search}", torrentSearchParams);

        while (!(_accessToken?.IsValid() ?? false))
        {
            await Login();
        }

        string token;

        while (!_accessToken.IsReadyForRequest(out token))
        {
            await Task.Delay(1000);
        }

        var requestUri = new Uri($"{_apiUrl}&{torrentSearchParams.AsQueryStringRarbg()}&token={token}");

        var searchResponse = await SendRequest<TorrentSearchRarbgResponseModel>(requestUri);

        if (searchResponse is null)
        {
            throw new HttpRequestException($"Request was unsuccessful");
        }

        return searchResponse.Torrents?.Select(r =>
        new TorrentInfo
        {
            Category = r.Category ?? "",
            Download = r.Download ?? "",
            Filename = r.Title ?? "",
            Seeders = r.Seeders.GetValueOrDefault(0).ToString(),
            Size = r.Size.GetValueOrDefault(0).ToString(),
        }).ToList() ?? new List<TorrentInfo>();
    }

    private async Task Login()
    {
        var requestUri = new Uri(_apiUrl + "&get_token=get_token");

        var accessToken = await SendRequest<TorrentsAccessTokenResponseModel>(requestUri);

        if (string.IsNullOrWhiteSpace(accessToken?.Token))
        {
            throw new HttpRequestException($"Request was unsuccessful");
        }

        _accessToken = new(accessToken.Token);
    }

    private async Task<T?> SendRequest<T>(Uri requestUri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var response = await _client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        var responseObject = JsonSerializer.Deserialize<T>(responseContent, _serializerOptions);

        return responseObject;
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
