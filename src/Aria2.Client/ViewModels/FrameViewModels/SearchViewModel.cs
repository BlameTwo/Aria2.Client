﻿using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using BtSearch.Loader.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IBtSearch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.FrameViewModels;

public sealed partial class SearchViewModel : ObservableRecipient
{
    public SearchViewModel(
        //[FromKeyedServices("Fitgril")] IBTSearchPlugin bTSearchPlugin,
        //[FromKeyedServices("1337x")]IBTSearchPlugin X1337plugin,
        IAria2cClient aria2CClient,
        IDataFactory dataFactory,IPluginManager pluginManager
    )
    {
        //FitgrilSearchPlugin = bTSearchPlugin;
        //X1337Plugin = X1337plugin;
        Aria2CClient = aria2CClient;
        DataFactory = dataFactory;
        PluginManager = pluginManager;
        this.Tabs = PluginManager.Plugins.ToList();

    }

    CancellationTokenSource cts = new CancellationTokenSource();

    [ObservableProperty]
    List<Tuple<PluginContextLoader, IBTSearchPlugin>> _Tabs;


    [ObservableProperty]
    Tuple<PluginContextLoader, IBTSearchPlugin> _SearchTag;

    [ObservableProperty]
    ObservableCollection<BTSearchRresultItem> _Result = new();

    //public IBTSearchPlugin FitgrilSearchPlugin { get; }
    //public IBTSearchPlugin X1337Plugin { get; }
    public IAria2cClient Aria2CClient { get; }
    public IDataFactory DataFactory { get; }
    public IPluginManager PluginManager { get; }

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
            
            await foreach (var item in SearchTag.Item2.SearchAsync(Query, cts.Token))
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

