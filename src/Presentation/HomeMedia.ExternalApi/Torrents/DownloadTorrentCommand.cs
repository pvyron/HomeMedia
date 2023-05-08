using HomeMedia.Application.Torrents.Interfaces;
using Mediator;

namespace HomeMedia.ExternalApi.Torrents;

public sealed record DownloadTorrentCommand(string Magnetlink, string? LocationName) : IRequest<IResult>;

public sealed class DownloadTorrentCommandHandler : IRequestHandler<DownloadTorrentCommand, IResult>
{
    private readonly IExternalTorrentQueueService _torrentQueueService;

    public DownloadTorrentCommandHandler(IExternalTorrentQueueService torrentQueueService)
    {
        _torrentQueueService = torrentQueueService;
    }

    public async ValueTask<IResult> Handle(DownloadTorrentCommand request, CancellationToken cancellationToken)
    {
        if (!request.Magnetlink.StartsWith("magnet:"))
            return Results.BadRequest("Invalid magnet link");

        try
        {
            await _torrentQueueService.AddTorrentToDownloadQueue(request.Magnetlink, Application.Torrents.Enums.TorrentSaveLocation.Any, cancellationToken);

            return Results.Ok();
        }
        catch
        {
            return Results.StatusCode(500);
        }
    }
}
