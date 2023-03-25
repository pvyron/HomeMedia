using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Application.Torrents.Models;
using HomeMedia.Infrastructure.Torrents.Models;
using HomeMedia.Models.Torrents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using HomeMedia.Infrastructure.Torrents.Dto;
using Microsoft.Extensions.Logging;

namespace HomeMedia.Application;
internal sealed class TorrentSearchService : ITorrentSearchService
{
    private readonly string _apiUrl = "https://torrentapi.org/pubapi_v2.php?app_id=vyron_torrent_app";

    private readonly ILogger<TorrentSearchService> _logger;
    private readonly HttpClient _client;
    private TorrentsApiAccessToken? _accessToken;

    public TorrentSearchService(ILogger<TorrentSearchService> logger, IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public async Task<IEnumerable<TorrentInfo>> QueryTorrentDataAsync(TorrentSearchParams torrentSearchParams)
    {
        while (!(_accessToken?.IsValid() ?? false))
        {
            await Login();
        }

        string token;

        while (!_accessToken.IsReadyForRequest(out token))
        {
            await Task.Delay(1000);
        }

        var requestUri = new Uri($"{_apiUrl}&{torrentSearchParams.AsQueryString()}&token={token}");

        var searchResponse = await SendRequest<TorrentSearchResponseModel>(requestUri);

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
            Seeders = r.Seeders.GetValueOrDefault(0),
            Size = r.Size.GetValueOrDefault(0),
        }) ?? new List<TorrentInfo>();
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
