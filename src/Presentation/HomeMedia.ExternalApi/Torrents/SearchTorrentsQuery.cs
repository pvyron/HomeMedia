using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Application.Torrents.Models;
using HomeMedia.Contracts.Torrents;
using Mediator;
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
        if (request.Model.Name.Length < 3)
            return Results.BadRequest("Torrent title must be at least 2 characters long");

        var query = request.Model.Name;

        query += " ";

        if (request.Model.Season is not null)
            query += $"S{request.Model.Season?.ToString("D2")}";

        if (request.Model.Episode is not null)
            query += $"E{request.Model.Episode?.ToString("D2")}";

        query = query.Trim();
        query += " ";

        if (request.Model.OnlyHdr.GetValueOrDefault(false))
            query += "HDR";

        query = query.Trim();
        query += " ";

        if (request.Model.OnlyWeb.GetValueOrDefault(false))
            query += "WEB";

        query = query.Trim();
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

        query = query.Trim();

        var searchParams = new TorrentSearchParams
        {
            QueryString = query,
            Page = 1
        };

        var results = new List<TorrentsSearchResponseModel>();
        await foreach (var torrent in _torrentSearchService.QueryTorrentDataAsync(searchParams, cancellationToken))
        {
            if (!torrent.MagnetLink.StartsWith("magnet:"))
                continue;

            results.Add(new TorrentsSearchResponseModel
            {
                Category = torrent.Category,
                Download = torrent.MagnetLink,
                Filename = torrent.Name,
                Seeders = torrent.Seeders,
                Size = torrent.Size,
                SizeText = torrent.Size
            });
        }

        return Results.Json(results, _jsonSerializerOptions);
    }
}
