using HomeMedia.MobileApp.Torrents;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HomeMedia.MobileApp.ViewModels;

namespace HomeMedia.MobileApp;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    private readonly ITorrentDataService _torrentDataService;

    public MainPage(ITorrentDataService torrentDataService)
    {
        _torrentDataService = torrentDataService;

        BindingContext = this;
        
        InitializeComponent();
    }

    async void OnSearchClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            await DisplayAlert("Error", "Must type name", "Ok");
            return;
        }

        Searching = true;
        Debug.WriteLine("Send request");
        try
        {
            var result = await _torrentDataService.SearchTorrentsAsync(SearchText);

            result.IfSucc(succ =>
            {
                Torrents = new();

                var i = 1;
                foreach (var torrent in succ)
                {
                    Torrents.Add(new TorrentSearchResultViewModel(i, torrent));
                    i++;
                }

                SearchResultsList.ItemsSource = Torrents;
            });
            result.IfFail(fail => Debug.WriteLine(fail.Message));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Warning", ex.Message, "Ok");
        }
        finally 
        {
            Debug.WriteLine("Request done");
            Searching = false; 
        }
    }

    async void OnTorrentSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await Task.CompletedTask;
        Debug.WriteLine("Torrent selected");
    }
}

public partial class MainPage
{
    private string _searchText = "";

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged(nameof(SearchText));
        }
    }

    bool _searching = false;

    public bool Searching
    {
        get => _searching;
        set
        {
            _searching = value;
            OnPropertyChanged(nameof(Searching));
            OnPropertyChanged(nameof(NotSearching));
        }
    }

    public bool NotSearching => !Searching;

    public ObservableCollection<TorrentSearchResultViewModel> Torrents { get; set; } = new ObservableCollection<TorrentSearchResultViewModel>()
    {
        new TorrentSearchResultViewModel
        {
            Id = 1,
            Category = "TestCategory",
            Filename = "Test Name!!",
            Seeders = "",
            Size = "",
            IsMagnet = true,
        },
        new TorrentSearchResultViewModel
        {
            Id = 2,
            Category = "TestCategory2",
            Filename = "Test Name@@",
            Seeders = "",
            Size = "",
            IsMagnet = false
        }
    };
}