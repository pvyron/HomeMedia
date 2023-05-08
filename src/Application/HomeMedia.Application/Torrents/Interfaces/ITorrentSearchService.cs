using HomeMedia.Application.Torrents.Models;
using HomeMedia.Models.Torrents;

namespace HomeMedia.Application.Torrents.Interfaces;
public interface ITorrentSearchService
{
    IAsyncEnumerable<TorrentInfo> QueryTorrentDataAsync(TorrentSearchParams torrentSearchParams, CancellationToken cancellationToken);
}