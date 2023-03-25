using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Infrastructure.Torrents.Models;
using HomeMedia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Application;
internal sealed class TorrentSearchService : ITorrentSearchService
{
    private string _apiUrl = "https://torrentapi.org/pubapi_v2.php";
    private readonly HttpClient _client;
    private TorrentsApiAccessToken? _accessToken;

    public TorrentSearchService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
    }

    public async Task<IEnumerable<TorrentData>> QueryTorrentDataAsync(string searchQuery)
    {
        while (!(_accessToken?.IsValid() ?? false))
        {
            await Login();
        }

        while (!_accessToken!.IsReadyForRequest(out var token))
        {
            await Task.Delay(5000);
        }

        var requestUri = new TorrentUri(_apiUrl, new Dictionary<string, string>
        {

        });

        

        return new List<TorrentData>();
    }

    private async Task Login()
    {
        var requestUri = new TorrentUri(_apiUrl, new Dictionary<string, string>
        {
            { "get_token", "get_token" }
        });

        var accessToken = await SendRequest<TorrentsAccessTokenResponseModel>(requestUri);

        if (string.IsNullOrWhiteSpace(accessToken?.Token))
        {
            throw new HttpRequestException($"Request was unsuccessful");
        }

        _accessToken = new(accessToken.Token);
    }

    private async Task<T?> SendRequest<T>(Uri requestUrl)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        var response = await _client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStreamAsync();

        var responseObject = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseContent);

        return responseObject;
    }
}
