using HomeMedia.Infrastructure.Torrents.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Options;
internal sealed class VHomeTorrentSearchServiceOptions
{
    public string BaseUrl { get; set; } = string.Empty;

    public const string SectionName = "TorrentSearchService";
}
