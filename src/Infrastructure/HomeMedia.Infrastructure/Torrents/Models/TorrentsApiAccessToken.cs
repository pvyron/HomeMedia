using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.Infrastructure.Torrents.Models;
internal sealed class TorrentsApiAccessToken
{
    public TorrentsApiAccessToken(string token)
    {
        _token = token;
        _createdOn = DateTime.UtcNow;
        _lastRequest = DateTime.UtcNow.AddMinutes(-1);
    }

    private readonly string _token;
    private readonly DateTime _createdOn;

    private DateTime _lastRequest;

    public bool IsValid() => _createdOn > DateTime.UtcNow.AddMinutes(14);

    public bool IsReadyForRequest(out string token)
    {
        token = string.Empty;

        if (_lastRequest < DateTime.UtcNow.AddSeconds(20))
            return false;

        _lastRequest = DateTime.UtcNow;
        token = _token;

        return true;
    }
}
