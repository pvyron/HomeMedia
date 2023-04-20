namespace HomeMedia.MobileApp.Torrents;

public interface ITorrentSearchService
{
    Task<List<TorrentModel>> SearchTorrents(string title);
}