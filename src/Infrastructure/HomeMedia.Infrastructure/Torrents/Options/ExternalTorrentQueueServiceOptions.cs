using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Options;
internal sealed class ExternalTorrentQueueServiceOptions
{
    //public ExternalTorrentQueueServiceOptions(IConfiguration configuration)
    //{
    //    ConnectionString = configuration.GetConnectionString("ExternalTorrentQueue")!;
    //    QueueName = configuration.GetValue<string>("ExternalTorrentQueue:Name")!;
    //}

    public string ConnectionString { get; set; } = null!;
    public string QueueName { get; set; } = null!;
}
