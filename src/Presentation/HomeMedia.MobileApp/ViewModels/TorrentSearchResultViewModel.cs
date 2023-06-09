﻿using HomeMedia.MobileApp.Torrents;
using System.Diagnostics.CodeAnalysis;

namespace HomeMedia.MobileApp.ViewModels;

public sealed class TorrentSearchResultViewModel
{
    public required TorrentModel Torrent;

    public TorrentSearchResultViewModel()
    {

    }

    [SetsRequiredMembers]
    public TorrentSearchResultViewModel(int id, TorrentModel torrentModel)
    {
        Id = id;
        Torrent = torrentModel;
        Filename = torrentModel.Filename;
        Category = torrentModel.Category;
        Seeders = torrentModel.Seeders;
        Size = torrentModel.Size;
        IsMagnet = torrentModel.IsMagnet();
    }

    public required int Id { get; init; }
    public required string Filename { get; init; }
    public required string Category { get; init; }
    public required string Seeders { get; init; }
    public required string Size { get; init; }
    public required bool IsMagnet { get; init; }

    public string IdText => $"{Id}";
    public string SeedsText => $"⚇ {Seeders}";
    public string SizeText => $"⛘ {Size}";
}
