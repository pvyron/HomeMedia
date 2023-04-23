using HomeMedia.MobileApp.Torrents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.MobileApp.ViewModels;
public sealed class TorrentSearchPageViewModel : INotifyPropertyChanged, INotifyPropertyChanging
{
	private string _searchText = "";

	public string SearchText
	{
		get => _searchText;
		set
		{
            PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(nameof(SearchText)));
            _searchText = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchText)));
        }
	}

	bool _searching = false;

	public bool Searching
	{
        get => _searching;
        set
        {
            PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(nameof(Searching)));
            PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(nameof(NotSearching)));
            _searching = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Searching)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotSearching)));
        }
    }

    public bool NotSearching => !Searching;

    public ObservableCollection<TorrentModel> Torrents { get; set; } = new();

    public event PropertyChangedEventHandler PropertyChanged;
    public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
}
