using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IBtSearch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.FrameViewModels;

public sealed partial class SearchViewModel : PageViewModelBase
{
    public SearchViewModel(
        //[FromKeyedServices("Fitgril")] IBTSearchPlugin bTSearchPlugin,
        //[FromKeyedServices("1337x")]IBTSearchPlugin X1337plugin,
        IAria2cClient aria2CClient,
        IDataFactory dataFactory,IPluginManager pluginManager
    ):base("搜索")
    {
        //FitgrilSearchPlugin = bTSearchPlugin;
        //X1337Plugin = X1337plugin;
        Aria2CClient = aria2CClient;
        DataFactory = dataFactory;
        PluginManager = pluginManager;

    }

    [RelayCommand]
    async Task Loaded()
    {
        await foreach (var item in PluginManager.GetSearchPlugins())
        {
            if (item == null)
                continue;
            if (item.Config!=null && item.Config.IsEnabled)
                Tabs.Add(item);
        }
    }

    CancellationTokenSource cts = new CancellationTokenSource();

    [ObservableProperty]
    List<IBTSearchPlugin> _Tabs=new();


    [ObservableProperty]
    IBTSearchPlugin _SearchTag;

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
            if (string.IsNullOrWhiteSpace(Query))
                return;
            IsRun = true;
            RunTip = "正在检索";
            Result.Clear();
            
            await foreach (var item in SearchTag.SearchAsync(Query, cts.Token))
            {
                if (item == null)
                    continue;
                Result.Add(DataFactory.CreateBTSearchRresultItem(item));
                RunTip =
                    $"当前检索位置{item.NowPage - 1}页，最大页面{item.MaxPageCount}，检索总数{Result.Count}";
            }
        }
        catch (Exception)
        {
            cts = new CancellationTokenSource();
        }
        RunTip = $"检索总数{Result.Count},开始搜索后清空";
        IsRun = false;
    }

    public override void Unregister()
    {
        cts.Cancel();
        Result.Clear();
        base.Unregister();
    }

    [RelayCommand]
    void ClearSearch()
    {
        cts.Cancel();
        IsRun = false;
    }
}

