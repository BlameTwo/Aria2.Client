using Aria2.Client.Models;
using Aria2.Client.Models.Anime;
using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Aria2.Client.ViewModels.FrameViewModels;

public sealed partial class AnimeViewModel:ObservableRecipient
{
    public AnimeViewModel(IOnekumaService rssService,IDataFactory dataFactory)
    {
        RssService = rssService;
        DataFactory = dataFactory;
    }

    public IOnekumaService RssService { get; }
    public IDataFactory DataFactory { get; }

    [RelayCommand]
    async void Loaded()
    {
        var result = await RssService.SearchKeyworkd(new List<string>() { "间谍过家家"}, "動畫");
        if (result == null)
        {
            return;
        }
        foreach (var item in result.Resources)
        {
            this.Resources.Add(DataFactory.CreateAnimeItemData(item));
        }
    }

    [ObservableProperty]
    ObservableCollection<AnimeItemData> _Resources=new();



}