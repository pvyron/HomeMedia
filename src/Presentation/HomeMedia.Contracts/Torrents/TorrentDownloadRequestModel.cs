namespace HomeMedia.Contracts.Torrents;
public sealed class TorrentDownloadRequestModel
{
    public required string MagnetLink { get; set; }

    public string? SaveLocation { get; set; }
}
