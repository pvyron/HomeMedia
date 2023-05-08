using HomeMedia.Application.Torrents.Enums;

namespace HomeMedia.Application.Torrents.Models;
public sealed class ExternalTorrentDownloadInfo
{
    public required string MagnetUrl { get; init; }
    public required TorrentSaveLocation TorrentSaveLocation { get; init; }
    public required string MessageId { get; init; }
    public required string PopReceipt { get; init; }
}
