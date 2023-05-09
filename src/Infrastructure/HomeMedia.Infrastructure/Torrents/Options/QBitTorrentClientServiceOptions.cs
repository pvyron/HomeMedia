using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Options;
internal sealed class QBitTorrentClientServiceOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string Referer { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public const string SectionName = "TorrentClientService";
}
