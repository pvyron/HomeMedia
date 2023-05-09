using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Infrastructure.Torrents.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Services;
internal sealed class QBitTorrentClientService : ITorrentClientService
{
    private readonly ILogger<QBitTorrentClientService> _logger;
    private readonly HttpClient _httpClient;
    private readonly QBitTorrentClientServiceOptions _options;

    public QBitTorrentClientService(ILogger<QBitTorrentClientService> logger, IOptions<QBitTorrentClientServiceOptions> options, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _options = options.Value;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("Referer", _options.Referer);
    }

    public async ValueTask<bool> DownloadTorrent(string magnetUrl, string path)
    {
        await Authenticate();

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/torrents/add");
        requestMessage.Content = new StringContent($"urls={magnetUrl}&savepath={path}");
        requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

        var response = await _httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
            return false;

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!responseContent.Contains("Ok"))
            return false;

        return true;
    }

    async ValueTask Authenticate()
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/auth/login");
        requestMessage.Content = new StringContent($"username={_options.Username}&password={_options.Password}");
        requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

        var response = await _httpClient.SendAsync(requestMessage);

        var responseContent = await response.Content.ReadAsStringAsync();
    }
}
