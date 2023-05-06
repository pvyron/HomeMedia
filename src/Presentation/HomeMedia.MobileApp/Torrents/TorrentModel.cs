using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.MobileApp.Torrents;

public sealed class TorrentModel
{
    public required string Filename { get; init; }
    private string _category;
    public required string Category
    {
        get => _category;
        init
        {
            _isMagnet = value.StartsWith("magnet:");
            _category = value;
        }
    }
    public required string Download { get; init; }
    public required string Size { get; init; }
    public required string SizeText { get; init; }
    public required string Seeders { get; init; }

    bool _isMagnet;
    public bool IsMagnet() => _isMagnet;
}
