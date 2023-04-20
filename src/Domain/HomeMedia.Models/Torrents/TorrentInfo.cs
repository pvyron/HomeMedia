namespace HomeMedia.Models.Torrents;
public sealed class TorrentInfo
{
    public required string Filename { get; init; }
    public required string Category { get; init; }
    public required string Download { get; init; }
    public required string Size { get; init; }
    public required string Seeders { get; init; }
}
