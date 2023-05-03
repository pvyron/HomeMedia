using HomeMedia.Application.Torrents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Application.Torrents.Interfaces;
public interface IExternalTorrentQueueService
{
    ValueTask AddTorrentToDownloadQueue(string magnetLink, TorrentSaveLocation saveLocation, CancellationToken cancellationToken);
}
