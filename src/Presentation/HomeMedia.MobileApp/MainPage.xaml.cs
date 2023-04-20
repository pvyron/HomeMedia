using HomeMedia.MobileApp.Torrents;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace HomeMedia.MobileApp;

public partial class MainPage : ContentPage
{
    private readonly TorrentSearchService _torrentSearchService;
    int _count = 0;

    public MainPage()
    {
        InitializeComponent();
        _torrentSearchService = new TorrentSearchService(new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            MaxDepth = 10,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true,
            DefaultBufferSize = 128
        });
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        _count++;

        if (_count == 1)
            CounterBtn.Text = $"Clicked {_count} time";
        else
            CounterBtn.Text = $"Clicked {_count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);

        _ = await _torrentSearchService.SearchTorrents("Ncis");

    }
}

