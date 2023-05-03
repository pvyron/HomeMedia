using HomeMedia.Application.Torrents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Models;
internal sealed class ExternalQueueTorrentModel
{
    public required string MagnetLink { get; set; }
    public required TorrentSaveLocation SaveLocation { get; set; }
}
