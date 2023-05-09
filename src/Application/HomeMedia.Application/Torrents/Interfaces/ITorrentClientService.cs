using HomeMedia.Models.Torrents;

namespace HomeMedia.Application.Torrents.Interfaces;
public interface ITorrentClientService
{
    ValueTask<bool> DownloadTorrent(string magnetUrl, string path);
}
