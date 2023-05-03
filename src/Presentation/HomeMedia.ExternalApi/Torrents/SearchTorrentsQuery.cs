﻿using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Application.Torrents.Models;
using HomeMedia.Contracts.Torrents;
using HomeMedia.Models.Torrents;
using LanguageExt;
using LanguageExt.Common;
using Mediator;
using System.Text;
using System.Text.Json;

namespace HomeMedia.ExternalApi.Torrents;

public sealed record SearchTorrentsQuery(TorrentsSearchRequestModel Model) : IRequest<IResult>;

public sealed class SearchTorrentsQueryHandler : IRequestHandler<SearchTorrentsQuery, IResult>
{
    private readonly ITorrentSearchService _torrentSearchService;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SearchTorrentsQueryHandler(ITorrentSearchService torrentSearchService, JsonSerializerOptions jsonSerializerOptions)
    {
        _torrentSearchService = torrentSearchService;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async ValueTask<IResult> Handle(SearchTorrentsQuery request, CancellationToken cancellationToken)
    {
        var query = request.Model.Name;

        query += " ";

        if (request.Model.Season is not null)
            query += $"S{request.Model.Season?.ToString("D2")}";

        if (request.Model.Episode is not null)
            query += $"E{request.Model.Episode?.ToString("D2")}";

        query.Trim();
        query += " ";

        switch (request.Model.Resolution)
        {
            case Resolution.sd_480p:
                query += "480p";
                break;
            case Resolution.hd_720p:
                query += "720p";
                break;
            case Resolution.fhd_1080p:
                query += "1080p";
                break;
            case Resolution.qhd_2160p:
                query += "2160p";
                break;
            default:
                break;
        }

        query.Trim();
        query += " ";

        if (request.Model.OnlyHdr.GetValueOrDefault(false))
            query += "HDR";

        query.Trim();
        query += " ";

        if (request.Model.OnlyWeb.GetValueOrDefault(false))
            query += "WEB";

        query.Trim();

        var searchParams = new TorrentSearchParams
        {
            QueryString = query, 
            Page = 1
        };

        var results = new List<TorrentInfo>();
        await foreach (var torrent in _torrentSearchService.QueryTorrentDataAsync(searchParams, cancellationToken))
        {
            if (!torrent.MagnetLink.StartsWith("magnet:"))
                continue;

            results.Add(torrent);
        }

        return Results.Json(results, _jsonSerializerOptions);
    }
}