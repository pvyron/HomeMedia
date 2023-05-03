using HomeMedia.Application.Torrents.Exceptions;
using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Application.Torrents.Models;
using HomeMedia.Infrastructure.Torrents.Dto;
using HomeMedia.Infrastructure.Torrents.Options;
using HomeMedia.Models.Torrents;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Services;
internal sealed class VHomeTorrentSearchService : ITorrentSearchService
{
    private readonly HttpClient _httpClient;
    private readonly VHomeTorrentSearchServiceOptions _options;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public VHomeTorrentSearchService(IHttpClientFactory httpClientFactory, IOptions<VHomeTorrentSearchServiceOptions> options, JsonSerializerOptions jsonSerializerOptions)
    {
        _options = options.Value;
        _httpClient = httpClientFactory.CreateClient();
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async IAsyncEnumerable<TorrentInfo> QueryTorrentDataAsync(TorrentSearchParams torrentSearchParams, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, SearchUrl(torrentSearchParams.QueryString, 1));

        request.Headers.Add("AuthKey", "1234");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(response.ReasonPhrase);
        }

        var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);

        await foreach (var torrentResponse in JsonSerializer.DeserializeAsyncEnumerable<VHomeTorrentSearchResponse>(responseContent, _jsonSerializerOptions, cancellationToken))
        {
            if (torrentResponse is null)
                continue;

            yield return new TorrentInfo
            {
                Category = torrentResponse.Category ?? "",
                MagnetLink = torrentResponse.Magnet ?? "",
                Name = torrentResponse.Name ?? "",
                Seeders = torrentResponse.Seeders ?? "",
                Size = torrentResponse.Size ?? ""
            };
        }
    }

    private string SearchUrl(string query, int page)
    {
        return $"{_options.BaseUrl}/1337x/search/{query}/{page}";
    }
}
