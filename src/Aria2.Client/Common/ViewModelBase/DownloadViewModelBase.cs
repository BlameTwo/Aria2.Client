using Aria2.Client.Models;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Models;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Client.Common.ViewModelBase;

public abstract partial class DownloadViewModelBase: PageViewModelBase
{
    public DownloadViewModelBase(IAria2cClient aria2CClient, IDataFactory dataFactory, IApplicationSetup<App> applicationSetup):base("下载")
    {
        Aria2CClient = aria2CClient;
        DataFactory = dataFactory;
        ApplicationSetup = applicationSetup;
        OnInitEnd();
        this.Downloads.CollectionChanged += Downloads_CollectionChanged;
    }

    private void Downloads_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        RefreshCard();
    }

    [ObservableProperty]
    double _CardVisibility = 1;


    internal void RefreshCard()
    {

        if (this.Downloads.Count == 0)
            CardVisibility = 1;
        else 
            CardVisibility = 0;
    }

    public CancellationTokenSource TokenSource = new();

    [ObservableProperty]
    ObservableCollection<DownloadTellItemData> _Downloads = new();

    public IAria2cClient Aria2CClient { get; }
    public IDataFactory DataFactory { get; }
    public IApplicationSetup<App> ApplicationSetup { get; }


    public void RemoveDownload(List<Param> gids)
    {
        if (gids == null || gids.Count == 0)
            return;
        ApplicationSetup.TryEnqueue(() =>
        {
            foreach (var item in Downloads.ToList())
            {
                foreach (var gid in gids)
                {
                    if (item.Data.Gid == gid.Gid)
                        Downloads.Remove(item);
                }
            }
        });
    }

    public void AddDownload(List<Param> gids)
    {
        ApplicationSetup.TryEnqueue(async () =>
        {
            if (gids == null || gids.Count == 0)
                return; 
            
            foreach (var item in gids)
            {
                var gid = await Aria2CClient.GetTellStatusAsync(item.Gid,TokenSource.Token);
                if (gid == null)
                    continue;
                if (gid.Result == null)
                    continue;
                var tell = DataFactory.CreateownloadTellItemData(gid.Result);
                if (this.Downloads.Count == 0)
                {
                    Downloads.Add(tell);
                    break;
                }
                foreach (var download in Downloads.ToList())
                {
                    if (item.Gid == download.Data.Gid) continue;
                    Downloads.Add(tell);
                }
            }
        });
    }


    public void AddDownload(DownloadTellItemData download)
    {
        ApplicationSetup.TryEnqueue(() =>
        {
            var list = this.Downloads.Where((x) => x.Data.Gid == download.Data.Gid);
            if (list.Count() == 0)
                this.Downloads.Add(download);
        });
    }


    [RelayCommand]
    async Task Loaded()
    {
        await OnRefreshAsync();
        OnLoaded();
    }


    [RelayCommand]
    void UnLoad() => OnUnLoad();

    public virtual void OnUnLoad()
    {
        this.Downloads.CollectionChanged -= Downloads_CollectionChanged;
        TokenSource.Cancel();
    }

    public abstract void OnInitEnd();

    public abstract Task OnRefreshAsync();

    public virtual void OnLoaded() { }
}
