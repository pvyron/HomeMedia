using HomeMedia.MobileApp.Torrents;
/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
Before:
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;
After:
using System.Text.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
*/

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-windows10.0.19041.0)'
Before:
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;
After:
using System.Text.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
*/

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-android)'
Before:
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;
After:
using System.Text.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
*/
using HomeMedia.MobileApp.ViewModels;
using System.Diagnostics;
/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
Before:
using System.Text.Json;

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
After:
/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
*/

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-windows10.0.19041.0)'
Before:
using System.Text.Json;

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
After:
/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
*/

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-android)'
Before:
using System.Text.Json;

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
After:
/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
*/


/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
Before:
using HomeMedia.MobileApp.ViewModels;
After:
using HomeMedia.Text.Json.Serialization;
*/

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-windows10.0.19041.0)'
Before:
using HomeMedia.MobileApp.ViewModels;
After:
using HomeMedia.Text.Json.Serialization;
*/

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-android)'
Before:
using HomeMedia.MobileApp.ViewModels;
After:
using HomeMedia.Text.Json.Serialization;
*/
/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-maccatalyst)'
Before:
using System.Diagnostics;
After:
using System.Diagnostics;
using System.Text.Json;
*/

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-windows10.0.19041.0)'
Before:
using System.Diagnostics;
After:
using System.Diagnostics;
using System.Text.Json;
*/

/* Unmerged change from project 'HomeMedia.MobileApp (net7.0-android)'
Before:
using System.Diagnostics;
After:
using System.Diagnostics;
using System.Text.Json;
*/


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

    private async void DownloadButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var torrwnt = ((sender as Button).BindingContext as TorrentSearchResultViewModel).Torrent;

            var result = await _torrentDataService.DownloadTorrentAsync(torrwnt.Download);
            result.IfFail(fail => Debug.WriteLine(fail.Message));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Warning", ex.Message, "Ok");
        }
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

    public ObservableCollection<TorrentSearchResultViewModel> Torrents { get; set; } = new();
}