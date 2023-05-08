namespace HomeMedia.Models.Torrents;
public sealed class TorrentInfo
{
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required string MagnetLink { get; init; }
    public required string Size { get; init; }
    public required string Seeders { get; init; }
}
