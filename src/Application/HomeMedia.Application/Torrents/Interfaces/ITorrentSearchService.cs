using HomeMedia.Models;

namespace HomeMedia.Application.Torrents.Interfaces;
public interface ITorrentSearchService
{
    Task<IEnumerable<TorrentData>> QueryTorrentDataAsync(string searchQuery);
}