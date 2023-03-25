namespace HomeMedia.Models;
public sealed class TorrentData
{
    public required string Filename { get; init; }
    public required string Category { get; init; }
    public required string Download { get; init; }
}
