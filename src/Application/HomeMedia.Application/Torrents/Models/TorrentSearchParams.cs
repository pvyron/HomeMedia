using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Application.Torrents.Models;
public sealed class TorrentSearchParams
{
    public required string Name { get; set; }
    public string? Category { get; set; }
}
