using System.Text.Json.Serialization;

namespace HomeMedia.Infrastructure.Torrents.Dto;
public sealed class TorrentSearchResponseModel
{
    [JsonPropertyName("torrent_results")]
    public List<TorrentResponseModel>? Torrents { get; set; }
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
    public uint? Seeders { get; set; }

    [JsonPropertyName("leechers")]
    public uint? Leechers { get; set; }

    [JsonPropertyName("size")]
    public ulong? Size { get; set; }

    [JsonPropertyName("pubdate")]
    public string? Pubdate { get; set; }

    [JsonPropertyName("episode_info")]
    public EpisodeInfo? EpisodeInfo { get; set; }

    [JsonPropertyName("ranked")]
    public short? Ranked { get; set; }

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

