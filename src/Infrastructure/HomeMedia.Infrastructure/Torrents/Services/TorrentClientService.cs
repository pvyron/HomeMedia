using HomeMedia.Application;
using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Models.Torrents;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Services;
internal sealed class TorrentClientService : ITorrentClientService
{
    private readonly string _torrentClientUrl = "http://localhost:782/api/v2";

    private readonly ILogger<TorrentClientService> _logger;
    private readonly HttpClient _client;

    public TorrentClientService(ILogger<TorrentClientService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _client = httpClientFactory.CreateClient();
        _client.DefaultRequestHeaders.Add("Referer", "http://localhost:782");
    }

    public async Task DownloadTorrent(string magnetUrl, string path)
    {
        await Authenticate();

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_torrentClientUrl}/torrents/add");
        requestMessage.Content = new StringContent($"urls={magnetUrl}&savepath={path}");
        requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

        var response = await _client.SendAsync(requestMessage);

        var responseContent = await response.Content.ReadAsStringAsync();
    }

    public async Task DownloadTorrent(string magnetUrl, TorrentType torrentType)
    {
        await Authenticate();

        var path = torrentType switch
        {
            TorrentType.Movie => "D:\\Media\\Movies",
            TorrentType.Series => "D:\\Media\\Series",
            _ => "D:\\Media"
        };

        await DownloadTorrent(magnetUrl, path);
    }

    async Task Authenticate()
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_torrentClientUrl}/auth/login");
        requestMessage.Content = new StringContent("username=admin&password=adminadmin");
        requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

        var response = await _client.SendAsync(requestMessage);

        var responseContent = await response.Content.ReadAsStringAsync();
    }
}
