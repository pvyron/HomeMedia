using HomeMedia.Models.Torrents;

namespace HomeMedia.Application.Torrents.Interfaces;
public interface ITorrentClientService
{
    Task DownloadTorrent(string magnetUrl, string path);
    Task DownloadTorrent(string magnetUrl, TorrentType torrentType);
}
