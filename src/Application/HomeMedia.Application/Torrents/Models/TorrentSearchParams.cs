using HomeMedia.Application.Torrents.Enums;
using HomeMedia.Application.Torrents.Extensions;
using System.Text;

namespace HomeMedia.Application.Torrents.Models;
public sealed class TorrentSearchParams
{
    public required string QueryString { get; set; }
    public int Page { get; set; } = 1;

    public string AsQueryStringRarbg()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append($"&search_string={QueryString}");

        return stringBuilder.ToString();
    }

    public string AsQueryStringRyuk()
    {
        return QueryString;
    }
}
