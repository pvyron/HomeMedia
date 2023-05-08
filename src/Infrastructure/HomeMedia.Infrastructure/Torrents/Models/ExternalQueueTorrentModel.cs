using HomeMedia.Application.Torrents.Enums;

namespace HomeMedia.Infrastructure.Torrents.Models;
internal sealed class ExternalQueueTorrentModel
{
    public required string MagnetLink { get; set; }
    public required TorrentSaveLocation SaveLocation { get; set; }
}
