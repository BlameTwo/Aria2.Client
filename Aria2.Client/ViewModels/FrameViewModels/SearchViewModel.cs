using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IBtSearch;
using IBtSearch.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.FrameViewModels;

public sealed partial class SearchViewModel:ObservableRecipient
{
    public SearchViewModel(
        [FromKeyedServices("Fitgril")] 
        IBTSearchPlugin bTSearchPlugin,
        IAria2cClient aria2CClient)
    {
        FitgrilSearchPlugin = bTSearchPlugin;
        Aria2CClient = aria2CClient;
    }

    [ObservableProperty]
    List<SearchTypeModel> _Tabs = new()
    {
        new()
        {
            Icon="https://fitgirl-repacks.site/wp-content/uploads/2016/08/cropped-icon-270x270.jpg",
            Name = "Fitgril",
            Tag = "Fitgril"
        }
    };

    [ObservableProperty]
    SearchTypeModel _SearchTag;

    [ObservableProperty]
    ObservableCollection<BTSearchResult> _Result=new();

    public IBTSearchPlugin FitgrilSearchPlugin { get; }
    public IAria2cClient Aria2CClient { get; }

    async partial void OnSearchTagChanged(SearchTypeModel value)
    {
        await this.refresh(value);
    }

    [ObservableProperty]
    string _Query;

    private async Task refresh(SearchTypeModel value)
    {
        switch (value.Tag)
        {
            case "Fitgril":
                break;
        }
    }

    [RelayCommand]
    async Task SearchAsync()
    {
        await foreach (var item in FitgrilSearchPlugin.SearchAsync(Query))
        {
            if (item == null)
                continue;
            this.Result.Add(item);
        }
    }

   
}


public class SearchTypeModel
{
    public string Name { get; set; }
    public string Tag { get; set; }

    public string Icon { get; set; }
}