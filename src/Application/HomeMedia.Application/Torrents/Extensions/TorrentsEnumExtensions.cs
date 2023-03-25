using HomeMedia.Application.Torrents.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Application.Torrents.Extensions;
public static class TorrentsEnumExtensions
{
    public static string ToQueryValue(this TorrentSearchCategoryEnum categoryEnum)
    {
        return categoryEnum switch
        {
            TorrentSearchCategoryEnum.Movies => "movies",
            TorrentSearchCategoryEnum.Series => "tv",
            _ => throw new NotImplementedException()
        };
    }
    public static string ToQueryValue(this TorrentSearchModeEnum modeEnum)
    {
        return modeEnum switch
        {
            TorrentSearchModeEnum.List => "list",
            TorrentSearchModeEnum.Search => "search",
            _ => throw new NotImplementedException()
        };
    }
    public static string ToQueryValue(this TorrentSearchResultFormatEnum resultFormatEnum)
    {
        return resultFormatEnum switch
        {
            TorrentSearchResultFormatEnum.Simple => "json",
            TorrentSearchResultFormatEnum.Extended => "json_extended",
            _ => throw new NotImplementedException()
        };
    }
    public static string ToQueryValue(this TorrentSearchResultLimitEnum resultLimitEnum)
    {
        return ((int)resultLimitEnum).ToString();
    }
    public static string ToQueryValue(this TorrentSearchResultSortingEnum resultSortingEnum)
    {
        return resultSortingEnum switch
        {
            TorrentSearchResultSortingEnum.Last => "last",
            TorrentSearchResultSortingEnum.Seeders => "seeders",
            TorrentSearchResultSortingEnum.Leechers => "leechers",
            _ => throw new NotImplementedException()
        };
    }
}
