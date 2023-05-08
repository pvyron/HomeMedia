using Azure.Storage.Queues;
using HomeMedia.Application.Torrents.Enums;
using HomeMedia.Application.Torrents.Interfaces;
using HomeMedia.Application.Torrents.Models;
using HomeMedia.Infrastructure.Torrents.Models;
using HomeMedia.Infrastructure.Torrents.Options;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;

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

    public async IAsyncEnumerable<ExternalTorrentDownloadInfo> GetPendingTorrents([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var response = await _queueClient.ReceiveMessagesAsync(maxMessages: 5, cancellationToken: cancellationToken);

        if (response is null)
            yield break;

        foreach (var queueMessage in response.Value)
        {
            if (queueMessage is null)
                continue;

            var messageContentStream = queueMessage.Body.ToStream();

            var queueModel = await JsonSerializer.DeserializeAsync<ExternalQueueTorrentModel>(messageContentStream, _jsonSerializerOptions, cancellationToken);

            if (queueModel is null)
                continue;

            yield return new ExternalTorrentDownloadInfo
            {
                MagnetUrl = queueModel.MagnetLink,
                MessageId = queueMessage.MessageId,
                PopReceipt = queueMessage.PopReceipt,
                TorrentSaveLocation = queueModel.SaveLocation
            };
        }
    }

    public async ValueTask RemoveTorrentFromQueue(string messageId, string popReceipt, CancellationToken cancellationToken)
    {
        await _queueClient.DeleteMessageAsync(messageId, popReceipt, cancellationToken);
    }
}
