using HomeMedia.MobileApp.Torrents;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;
using HomeMedia.MobileApp.ViewModels;
using System.Collections.ObjectModel;

namespace HomeMedia.MobileApp;

public partial class MainPage : ContentPage
{
    private readonly ITorrentDataService _torrentDataService;
    private readonly TorrentSearchPageViewModel _viewModel;

    public MainPage(ITorrentDataService torrentDataService)
    {
        InitializeComponent();

        _viewModel = new TorrentSearchPageViewModel();
        _torrentDataService = torrentDataService;

        BindingContext = _viewModel;
    }

    async void OnSearchClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_viewModel.SearchText))
        {
            await DisplayAlert("Error", "Must type name", "Ok");
            return;
        }

        _viewModel.Searching = true;

        try
        {
            var result = await _torrentDataService.SearchTorrentsAsync(_viewModel.SearchText);

            result.IfSucc(succ =>
            {
                _viewModel.Torrents = new();

                foreach (var torrent in succ)
                {
                    _viewModel.Torrents.Add(torrent);
                }
            });
            result.IfFail(fail => Debug.WriteLine(fail.Message));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Warning", ex.Message, "Ok");
        }
        finally 
        { 
            _viewModel.Searching = false; 
        }
    }

    async void OnTorrentSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await Task.CompletedTask;
        Debug.WriteLine("Torrent selected");
    }
}

