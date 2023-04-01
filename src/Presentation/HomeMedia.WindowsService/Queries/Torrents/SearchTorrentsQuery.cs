using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Application.Torrents.Models;
using HomeMedia.Contracts.Torrents;
using HomeMedia.Infrastructure.Tools;
using LanguageExt.Common;
using Mediator;

namespace HomeMedia.WindowsService.Queries.Torrents;

public sealed record SearchTorrentsQuery(string Query) : IRequest<Result<IEnumerable<TorrentsSearchResponseModel>>>;

public sealed class SearchTorrentsQueryHandler : IRequestHandler<SearchTorrentsQuery, Result<IEnumerable<TorrentsSearchResponseModel>>>
{
    private readonly ILogger<SearchTorrentsQueryHandler> _logger;
    private readonly ITorrentSearchService _torrentSearch;

    public SearchTorrentsQueryHandler(ILogger<SearchTorrentsQueryHandler> logger, ITorrentSearchService torrentSearch)
    {
        _logger = logger;
        _torrentSearch = torrentSearch;
    }

    public async ValueTask<Result<IEnumerable<TorrentsSearchResponseModel>>> Handle(SearchTorrentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var searchQuery = request.Query.Replace(' ', ';');

            var torrentSearchParams = new TorrentSearchParams
            {
                Name = searchQuery
            };

            var torrentInfos = await _torrentSearch.QueryTorrentDataAsync(torrentSearchParams);

            var response = torrentInfos.Select(info =>
            {
                return new TorrentsSearchResponseModel
                {
                    Category = info.Category,
                    Download = info.Download,
                    Filename = info.Filename,
                    Seeders = info.Seeders,
                    Size = info.Size,
                    SizeText = ((long)info.Size).GetBytesReadable()
                };
            });

            return new Result<IEnumerable<TorrentsSearchResponseModel>>(response);
        }
        catch (Exception ex)
        {
            return new Result<IEnumerable<TorrentsSearchResponseModel>>(ex);
        }
    }
}
