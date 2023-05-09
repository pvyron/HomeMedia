using HomeMedia.Application.Torrents.Interfaces;

namespace HomeMedia.PiProxy.Torrents;

public sealed class TorrentDownloadWorker : BackgroundService
{
    private readonly PeriodicTimer _timer = new (TimeSpan.FromMinutes(1));

    private readonly ILogger<TorrentDownloadWorker> _logger;
    private readonly IExternalTorrentQueueService _externalTorrentQueueService;

    public TorrentDownloadWorker(ILogger<TorrentDownloadWorker> logger, IExternalTorrentQueueService externalTorrentQueueService)
    {
        _logger = logger;
        _externalTorrentQueueService = externalTorrentQueueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            await foreach (var torrentInfo in _externalTorrentQueueService.GetPendingTorrents(stoppingToken))
            {
                await Task.Delay(1000);

                if (true)
                    continue;

                await _externalTorrentQueueService.RemoveTorrentFromQueue(torrentInfo.MessageId, torrentInfo.PopReceipt, stoppingToken);
            }
        }
    }
}
