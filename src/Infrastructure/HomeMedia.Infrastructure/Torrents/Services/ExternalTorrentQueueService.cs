using Azure.Storage.Queues;
using HomeMedia.Application.Torrents.Enums;
using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Infrastructure.Torrents.Models;
using HomeMedia.Infrastructure.Torrents.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Services;
internal sealed class ExternalTorrentQueueService : IExternalTorrentQueueService
{
    private readonly QueueClient _queueClient;
    private readonly ExternalTorrentQueueServiceOptions _options;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ExternalTorrentQueueService(IOptions<ExternalTorrentQueueServiceOptions> options, JsonSerializerOptions jsonSerializerOptions)
    {
        _options = options.Value;
        _queueClient = new QueueClient(_options.ConnectionString, _options.QueueName);
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async ValueTask AddTorrentToDownloadQueue(string magnetLink, TorrentSaveLocation saveLocation, CancellationToken cancellationToken)
    {
        var queueModel = new ExternalQueueTorrentModel { MagnetLink = magnetLink, SaveLocation = saveLocation };

        var queueModelSerialized = JsonSerializer.Serialize(queueModel, _jsonSerializerOptions);

        await _queueClient.SendMessageAsync(queueModelSerialized, cancellationToken);
    }
}
