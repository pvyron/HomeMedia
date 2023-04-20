using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMedia.MobileApp.ViewModels;
public sealed class TorrentSearchPageViewModel : INotifyPropertyChanged, INotifyPropertyChanging
{
	private string _searchText;

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

	public event PropertyChangedEventHandler PropertyChanged;
    public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
}
