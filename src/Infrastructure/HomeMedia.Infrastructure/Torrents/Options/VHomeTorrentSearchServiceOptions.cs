namespace HomeMedia.Infrastructure.Torrents.Options;
internal sealed class VHomeTorrentSearchServiceOptions
{
    public string BaseUrl { get; set; } = string.Empty;

    public const string SectionName = "TorrentSearchService";
}
