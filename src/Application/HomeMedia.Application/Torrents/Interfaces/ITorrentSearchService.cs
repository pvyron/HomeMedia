using HomeMedia.Application.Torrents.Models;
using HomeMedia.Models.Torrents;

namespace HomeMedia.Application.Torrents.Interfaces;
public interface ITorrentSearchService
{
    Task<List<TorrentInfo>> QueryTorrentDataAsync(TorrentSearchParams torrentSearchParams);
}