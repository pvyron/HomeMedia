using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Models;
public sealed class TorrentUri : Uri
{
    public TorrentUri([StringSyntax("Uri")] string uriString, Dictionary<string, string> queryValues) : base(CombineUri(uriString, queryValues))
    {

    }

    private static string CombineUri(string uriString, Dictionary<string, string> queryValues)
    {
        var queryBuilder = new StringBuilder();
        queryBuilder.Append("?app_id=vyron_torrent_app");

        foreach (var queryValue in queryValues)
        {
            queryBuilder.Append($"&{queryValue.Key}={queryValue.Value}");
        }

        var combinedUri = queryBuilder.ToString();

        return combinedUri;
    }
}
