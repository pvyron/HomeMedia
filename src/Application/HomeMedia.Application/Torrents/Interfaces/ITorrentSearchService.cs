using HomeMedia.Application.Torrents.Models;
using HomeMedia.Models.Torrents;
using System.Runtime.CompilerServices;

namespace HomeMedia.Application.Torrents.Interfaces;
public interface ITorrentSearchService
{
    IAsyncEnumerable<TorrentInfo> QueryTorrentDataAsync(TorrentSearchParams torrentSearchParams, CancellationToken cancellationToken);
}