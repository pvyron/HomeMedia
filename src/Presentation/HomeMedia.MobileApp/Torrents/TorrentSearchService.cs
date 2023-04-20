
using HomeMedia.Contracts.Torrents;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HomeMedia.MobileApp.Torrents;

public sealed class TorrentSearchService : ITorrentSearchService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public TorrentSearchService(JsonSerializerOptions jsonSerializerOptions)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://192.168.1.200:4200/api/torrents/search")
        };

        _serializerOptions = jsonSerializerOptions;
        _serializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public async Task<List<TorrentModel>> SearchTorrents(string title)
    {
        try
        {
            var requestModel = new TorrentsSearchRequestModel
            {
                Query = title
            };

            var response = await _httpClient.PostAsJsonAsync("", requestModel, _serializerOptions, CancellationToken.None);

            var torrents = await JsonSerializer.DeserializeAsync<List<TorrentsSearchResponseModel>>(await response.Content.ReadAsStreamAsync(), _serializerOptions, CancellationToken.None);

            return torrents.Select(t => new TorrentModel
            {
                Category = t.Category,
                Download = t.Download,
                Filename = t.Filename,
                Seeders = t.Seeders,
                Size = t.Size,
                SizeText = t.SizeText
            }).ToList();
        }
        catch 
        {

        }

        return new List<TorrentModel>();
    }
}
