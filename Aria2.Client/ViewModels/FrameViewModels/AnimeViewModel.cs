using Aria2.Client.Models;
using Aria2.Client.Models.Anime;
using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Aria2.Client.ViewModels.FrameViewModels;

public sealed partial class AnimeViewModel:ObservableRecipient
{
    public AnimeViewModel(IRssService rssService,IDataFactory dataFactory)
    {
        RssService = rssService;
        DataFactory = dataFactory;
    }

    public IRssService RssService { get; }
    public IDataFactory DataFactory { get; }

    [RelayCommand]
    async void Loaded()
    {
        var result = await RssService.GetAnimeHomeAsync(1,10);
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