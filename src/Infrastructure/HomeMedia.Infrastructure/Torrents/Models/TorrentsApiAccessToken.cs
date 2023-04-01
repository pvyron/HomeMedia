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

    public bool IsValid() => _createdOn.AddMinutes(14) >= DateTime.UtcNow;

    public bool IsReadyForRequest(out string token)
    {
        token = string.Empty;

        if (_lastRequest.AddSeconds(20) >= DateTime.UtcNow)
            return false;

        _lastRequest = DateTime.UtcNow;
        token = _token;

        return true;
    }
}
