﻿using Aria2.Client.Models;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
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

public sealed partial class SearchViewModel : ObservableRecipient
{
    public SearchViewModel(
        [FromKeyedServices("Fitgril")] IBTSearchPlugin bTSearchPlugin,
        [FromKeyedServices("1337x")]IBTSearchPlugin X1337plugin,
        IAria2cClient aria2CClient,
        IDataFactory dataFactory
    )
    {
        FitgrilSearchPlugin = bTSearchPlugin;
        X1337Plugin = X1337plugin;
        Aria2CClient = aria2CClient;
        DataFactory = dataFactory;
    }

    CancellationTokenSource cts = new CancellationTokenSource();

    [ObservableProperty]
    List<SearchTypeModel> _Tabs =
        new()
        {
            new()
            {
                Icon =
                    "https://fitgirl-repacks.site/wp-content/uploads/2016/08/cropped-icon-270x270.jpg",
                Name = "Fitgril",
                Tag = "Fitgril"
            },
            new()
            {
                Icon="https://www.1337xx.to/favicon.ico",
                Name="1337X",
                Tag="1337X"
            }
        };

    [ObservableProperty]
    SearchTypeModel _SearchTag;

    [ObservableProperty]
    ObservableCollection<BTSearchRresultItem> _Result = new();

    public IBTSearchPlugin FitgrilSearchPlugin { get; }
    public IBTSearchPlugin X1337Plugin { get; }
    public IAria2cClient Aria2CClient { get; }
    public IDataFactory DataFactory { get; }

    [ObservableProperty]
    string _Query;

    [ObservableProperty]
    bool _IsRun;

    [ObservableProperty]
    string _RunTip = "请输入关键字进行搜索";

    [RelayCommand]
    async Task SearchAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(this.Query))
                return;
            IsRun = true;
            this.RunTip = "正在检索";
            this.Result.Clear();
            IBTSearchPlugin _searchPlugin = default;
            if (this.SearchTag.Tag == "Fitgril")
            {
                _searchPlugin = FitgrilSearchPlugin;
            }
            else if(this.SearchTag.Tag == "1337X")
            {
                _searchPlugin = X1337Plugin;
            }
            if (_searchPlugin == null)
                return;
            await foreach (var item in _searchPlugin.SearchAsync(Query, cts.Token))
            {
                if (item == null)
                    continue;
                this.Result.Add(DataFactory.CreateBTSearchRresultItem(item));
                this.RunTip =
                    $"当前检索位置{item.NowPage - 1}页，最大页面{item.MaxPageCount}，检索总数{Result.Count}";
            }
        }
        catch (Exception)
        {
            this.cts = new CancellationTokenSource();
        }
        this.RunTip = $"检索总数{Result.Count},开始搜索后清空";
        IsRun = false;
    }

    [RelayCommand]
    void ClearSearch()
    {
        cts.Cancel();
        IsRun = false;
    }
}

public class SearchTypeModel
{
    public string Name { get; set; }
    public string Tag { get; set; }

    public string Icon { get; set; }
}