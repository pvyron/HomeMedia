using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using HomeMedia.Contracts.Torrents;
using System.Net.Http.Json;
using LanguageExt.Common;
using LanguageExt;
using System.Diagnostics;

namespace HomeMedia.MobileApp.Torrents;
public interface ITorrentDataService
{
    ValueTask<Result<IEnumerable<TorrentModel>>> SearchTorrentsAsync(string query);
    ValueTask<Result<Unit>> DownloadTorrentAsync(string magnetLink);
}

public sealed class TorrentDataService : ITorrentDataService
{
    private readonly string _searchUrl;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public TorrentDataService()
    {
        _searchUrl = "https://pvmediasearch.azurewebsites.net/api/SearchMedia?code=cAQorMoosddW_Onmzi7gUTg89b9sq0_ZGT6GeTEfUNE5AzFuqmohrQ==";
        //_searchUrl = "http://localhost:7027/api/SearchMedia";
        _httpClient = new HttpClient();

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = false,
        };
    }

    public async ValueTask<Result<Unit>> DownloadTorrentAsync(string magnetLink)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async ValueTask<Result<IEnumerable<TorrentModel>>> SearchTorrentsAsync(string query)
    {
        try
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return new Result<IEnumerable<TorrentModel>>(new Exception("No internet access"));
            }

            var requestModel = new TorrentsSearchRequestModel
            {
                Query = query
            };

            var jsonModel = JsonSerializer.Serialize(requestModel, _serializerOptions);

            var requestContent = new StringContent(jsonModel, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_searchUrl, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                return new Result<IEnumerable<TorrentModel>>(new Exception($"{response.StatusCode} - {response.ReasonPhrase}"));
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var torrents = JsonSerializer.Deserialize<List<TorrentsSearchResponseModel>>(responseContent, _serializerOptions);

            return torrents.Select(t => new TorrentModel
            {
                Category = t.Category,
                Download = t.Download,
                Filename = t.Filename,
                Seeders = t.Seeders,
                Size = t.Size,
                SizeText = t.SizeText
            }).ToHashSet();
        }
        catch (Exception ex)
        {
            return new Result<IEnumerable<TorrentModel>>(ex);
        }
    }
}