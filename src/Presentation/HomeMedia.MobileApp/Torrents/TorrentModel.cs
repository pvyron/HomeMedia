using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.MobileApp.Torrents;

public sealed class TorrentModel
{
    public required string Filename { get; init; }
    public required string Category { get; init; }
    public required string Download { get; init; }
    public required string Size { get; init; }
    public required string SizeText { get; init; }
    public required string Seeders { get; init; }
}
