using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Dto;
internal sealed class VHomeTorrentSearchResponse
{
    public string? Name { get; set; }
    public string? Magnet { get; set; }
    public string? Poster { get; set; }
    public string? Category { get; set; }
    public string? Type { get; set; }
    public string? Language { get; set; }
    public string? Size { get; set; }
    public string? UploadedBy { get; set; }
    public string? Downloads { get; set; }
    public string? LastChecked { get; set; }
    public string? DateUploaded { get; set; }
    public string? Seeders { get; set; }
    public string? Leechers { get; set; }
    public string? Url { get; set; }
}
