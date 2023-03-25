using HomeMedia.Models.Torrents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Application.Torrents.Interfaces;
public interface ITorrentClientService
{
    Task DownloadTorrent(string magnetUrl, string path);
    Task DownloadTorrent(string magnetUrl, TorrentType torrentType);
}
