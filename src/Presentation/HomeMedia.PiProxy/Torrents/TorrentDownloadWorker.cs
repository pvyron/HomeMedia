using HomeMedia.Application.Torrents.Enums;
using HomeMedia.Application.Torrents.Interfaces;

namespace HomeMedia.PiProxy.Torrents;

public sealed class TorrentDownloadWorker : BackgroundService
{
    private readonly PeriodicTimer _timer = new (TimeSpan.FromSeconds(10));

    private readonly ILogger<TorrentDownloadWorker> _logger;
    private readonly IExternalTorrentQueueService _externalTorrentQueueService;
    private readonly ITorrentClientService _torrentClientService;

    public TorrentDownloadWorker(ILogger<TorrentDownloadWorker> logger, IExternalTorrentQueueService externalTorrentQueueService, ITorrentClientService torrentClientService)
    {
        _logger = logger;
        _externalTorrentQueueService = externalTorrentQueueService;
        _torrentClientService = torrentClientService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            await foreach (var torrentInfo in _externalTorrentQueueService.GetPendingTorrents(stoppingToken))
            {
                var path = GetPath(torrentInfo.TorrentSaveLocation);

                if (!await _torrentClientService.DownloadTorrent(torrentInfo.MagnetUrl, path))
                    continue;

                await _externalTorrentQueueService.RemoveTorrentFromQueue(torrentInfo.MessageId, torrentInfo.PopReceipt, stoppingToken);
            }
        }
    }

    string GetPath(TorrentSaveLocation torrentSaveLocation)
    {
        return torrentSaveLocation switch
        {
            TorrentSaveLocation.Series => "D:\\Media\\Series",
            TorrentSaveLocation.Movies => "D:\\Media\\Movies",
            _ => "D:\\Media"
        };
    }
}
