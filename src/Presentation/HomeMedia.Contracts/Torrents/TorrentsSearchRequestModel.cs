namespace HomeMedia.Contracts.Torrents;
public sealed class TorrentsSearchRequestModel
{
    public required string Name { get; set; }
    public int? Season { get; set; }
    public int? Episode { get; set; }
    public Resolution Resolution { get; set; } = Resolution.Any;
    public bool? OnlyHdr { get; set; }
    public bool? OnlyWeb { get; set; }

    public int Page { get; set; } = 1;
}

public enum Resolution
{
    Any,
    Max,
    sd_480p,
    hd_720p,
    fhd_1080p,
    qhd_2160p,
}