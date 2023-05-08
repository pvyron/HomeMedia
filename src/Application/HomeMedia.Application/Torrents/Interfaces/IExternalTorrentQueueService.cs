using HomeMedia.Application.Torrents.Enums;
using HomeMedia.Application.Torrents.Models;

namespace HomeMedia.Application.Torrents.Interfaces;
public interface IExternalTorrentQueueService
{
    ValueTask AddTorrentToDownloadQueue(string magnetLink, TorrentSaveLocation saveLocation, CancellationToken cancellationToken);

    IAsyncEnumerable<ExternalTorrentDownloadInfo> GetPendingTorrents(CancellationToken cancellationToken);

    ValueTask RemoveTorrentFromQueue(string messageId, string popReceipt, CancellationToken cancellationToken);
}
