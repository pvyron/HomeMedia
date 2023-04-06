using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Contracts.Torrents;
public sealed class TorrentsSearchResponseModel
{
    public required string Filename { get; init; }
    public required string Category { get; init; }
    public required string Download { get; init; }
    public required ulong Size { get; init; }
    public required string SizeText { get; init; }
    public required uint Seeders { get; init; }
}