using HomeMedia.MobileApp.Torrents;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;

namespace HomeMedia.MobileApp;

public partial class MainPage : ContentPage
{
    private readonly ITorrentDataService _torrentDataService;

    public MainPage(ITorrentDataService torrentDataService)
    {
        InitializeComponent();

        _torrentDataService = torrentDataService;
    }

    async void OnSearchClicked(object sender, EventArgs e)
    {
        var result = await _torrentDataService.SearchTorrentsAsync("young sheldon");

        result.IfSucc(succ => searchResultsCollectionView.ItemsSource = succ.ToList());
        result.IfFail(fail => Debug.WriteLine(fail.Message));
    }

    async void OnTorrentSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Debug.WriteLine("Torrent selected");
    }
}

