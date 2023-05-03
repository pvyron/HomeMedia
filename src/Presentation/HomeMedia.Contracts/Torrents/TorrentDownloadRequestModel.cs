using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Contracts.Torrents;
public sealed class TorrentDownloadRequestModel
{
    public required string MagnetLink { get; set; }

    public string? SaveLocation { get; set; }
}
