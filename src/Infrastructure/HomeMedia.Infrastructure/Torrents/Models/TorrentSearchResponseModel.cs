using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Models;
public sealed class TorrentSearchResponseModel
{
    [JsonPropertyName("torrent_results")]
    public List<TorrentSearchResponseModel>? Torrents { get; set; }
}

public sealed class TorrentResponseModel
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("download")]
    public string? Download { get; set; }

    [JsonPropertyName("seeders")]
    public int? Seeders { get; set; }

    [JsonPropertyName("leechers")]
    public int? Leechers { get; set; }

    [JsonPropertyName("size")]
    public int? Size { get; set; }

    [JsonPropertyName("pubdate")]
    public string? Pubdate { get; set; }

    [JsonPropertyName("episode_info")]
    public EpisodeInfo? EpisodeInfo { get; set; }

    [JsonPropertyName("ranked")]
    public int? Ranked { get; set; }

    [JsonPropertyName("info_page")]
    public string? InfoPage { get; set; }
}
public sealed class EpisodeInfo
{
    [JsonPropertyName("imdb")]
    public string? Imdb { get; set; }

    [JsonPropertyName("tvrage")]
    public string? Tvrage { get; set; }

    [JsonPropertyName("tvdb")]
    public string? Tvdb { get; set; }

    [JsonPropertyName("themoviedb")]
    public string? Theoviedb { get; set; }
}

