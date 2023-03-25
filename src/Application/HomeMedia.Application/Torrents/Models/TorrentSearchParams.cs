using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMedia.Application.Torrents.Enums;
using HomeMedia.Application.Torrents.Extensions;

namespace HomeMedia.Application.Torrents.Models;
public sealed class TorrentSearchParams
{
    public required string Name { get; set; }
    public uint MinSeeders { get; set; } = 0;
    public uint MinLeechers { get; set; } = 0;
    public bool OnlyRanked { get; set; } = true;
    public TorrentSearchModeEnum Mode { get; set; } = TorrentSearchModeEnum.Search;
    public TorrentSearchCategoryEnum Category { get; set; } = TorrentSearchCategoryEnum.None;
    public TorrentSearchResultSortingEnum SortBy { get; set; } = TorrentSearchResultSortingEnum.Last;
    public TorrentSearchResultLimitEnum ResultsLimit { get; set; } = TorrentSearchResultLimitEnum.Min25;
    public TorrentSearchResultFormatEnum ResultsFormat { get; set; } = TorrentSearchResultFormatEnum.Extended;

    public string AsQueryString()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append($"mode={Mode.ToQueryValue()}");
        stringBuilder.Append($"&search_string={Name}");

        if (Category != TorrentSearchCategoryEnum.None)
            stringBuilder.Append($"&category={Category.ToQueryValue()}");

        stringBuilder.Append($"&limit={ResultsLimit.ToQueryValue()}");
        stringBuilder.Append($"&sort={SortBy.ToQueryValue()}");

        if (MinSeeders > 0)
            stringBuilder.Append($"&min_seeders={MinSeeders}");

        if (MinLeechers > 0)
            stringBuilder.Append($"&min_leechers={MinLeechers}");

        stringBuilder.Append($"&format={ResultsFormat.ToQueryValue()}");

        if (!OnlyRanked)
            stringBuilder.Append($"&ranked=0");

        return stringBuilder.ToString();
    }
}
