using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Models.ClientModel;
using Aria2.Net.Services.Contracts;
using Aria2.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Aria2.Client.ViewModels.SplitViewModels;

public sealed partial  class TellDownloadSessionViewModel:ObservableObject
{
    CancellationTokenSource _source = new CancellationTokenSource();

    public TellDownloadSessionViewModel(
        IAria2cClient aria2CClient,
        IDialogManager dialogManager,
        IDataFactory dataFactory
    )
    {
        Aria2CClient = aria2CClient;
        DialogManager = dialogManager;
        DataFactory = dataFactory;
    }

    public IAria2cClient Aria2CClient { get; }
    public IDialogManager DialogManager { get; }
    public IDataFactory DataFactory { get; }
    public string Gid { get; private set; }

    DispatcherTimer _timer = null;

    [ObservableProperty]
    ObservableCollection<DownloadFile> _Downloads;

    [ObservableProperty]
    DownloadTellItemData _Data;

    [ObservableProperty]
    SelectorBarItem _TabSelectoritem;

    [ObservableProperty]
    Visibility _TaskVisibility = Visibility.Visible;

    [ObservableProperty]
    Visibility _LinkVisibility = Visibility.Collapsed;

    [ObservableProperty]
    Visibility _DataVisibility = Visibility.Collapsed;

    [ObservableProperty]
    Visibility _TrackerVisibility = Visibility.Collapsed;

    [ObservableProperty]
    ObservableCollection<BittorrentPeer> _Peers;

    [ObservableProperty]
    ObservableCollection<string> _Trackers;

    [ObservableProperty]
    int _SelectFile;

    [ObservableProperty]
    int _Size;

    [RelayCommand]
    async Task RefreshDownloads() => await refreshDownloads();

    [RelayCommand]
    void CloseDialog()
    {
        _timer.Stop();
        _timer = null;
        Peers = null;
        Data = null;
        _source.Cancel();
        DialogManager.CloseDialog();
    }

    async Task Refresh()
    {
        if (Data != null)
        {
            var ips = await Aria2CClient.GetBittorrentPeers(Gid, _source.Token);
            if (ips == null)
            {
                return;
            }
            Peers = new(ips.Result);
        }
    }

    partial void OnTabSelectoritemChanged(SelectorBarItem value)
    {
        if (value.Tag.ToString() == "Detlis")
        {
            TaskVisibility = Visibility.Visible;
            LinkVisibility = Visibility.Collapsed;
            TrackerVisibility = Visibility.Collapsed;
            DataVisibility = Visibility.Collapsed;
        }
        else if (value.Tag.ToString() == "IPLink")
        {
            TaskVisibility = Visibility.Collapsed;
            LinkVisibility = Visibility.Visible;
            TrackerVisibility = Visibility.Collapsed;
            DataVisibility = Visibility.Collapsed;
        }
        else if (value.Tag.ToString() == "Data")
        {

            TaskVisibility = Visibility.Collapsed;
            LinkVisibility = Visibility.Collapsed;
            TrackerVisibility = Visibility.Collapsed;
            DataVisibility = Visibility.Visible;
        }else if(value.Tag.ToString() == "Tracker")
        {
            TaskVisibility = Visibility.Collapsed;
            LinkVisibility = Visibility.Collapsed;
            TrackerVisibility = Visibility.Visible;
            DataVisibility = Visibility.Collapsed;
        }
    }

    public async Task RefreshTask(string gidName)
    {
        Gid = gidName;
        var data = await Aria2CClient.GetTellStatusAsync(Gid, _source.Token);
        Data = DataFactory.CreateownloadTellItemData(data.Result);
        RefreshTracker();
        _timer = new DispatcherTimer();
        _timer.Tick += _timer_Tick;
        _timer.Interval = new(0, 0, 1);
        _timer.Start();
    }

    private void RefreshTracker()
    {
        if (Data.Data.Bittorrent == null)
            return;
        List<string> trackers = new();
        foreach (var item in Data.Data.Bittorrent.AnnounceList)
        {
            trackers.AddRange(item);
        }
        Trackers = new(trackers);
    }

    private async Task refreshDownloads()
    {
        if (Data.Data.Bittorrent == null)
            return;
        var downloads = await Aria2CClient.GetFiles(Gid, _source.Token);
        if (downloads == null)
            return;
        Downloads = new(downloads.Result);
        SelectFile = Downloads.Count();
        Size = 2312;
    }

    [RelayCommand]
    async Task EditerTask()
    {
        var pauseResult = await Aria2CClient.ForcePause(Gid, _source.Token);
        if (pauseResult == null)
            return;
        string numbers = "";
        List<int> list = new List<int>();
        Downloads.ToList().ForEach((v) =>
        {
            if (v.Selected == "true" || v.Selected == "True")
            {
                list.Add(int.Parse(v.Index));
            }
        });
        numbers = string.Join(",", list);
        var state = await Aria2CClient.GetTellStatusAsync(pauseResult.Result, _source.Token);
        if (state.Result.Status == TellState.Paused)
        {
            var data = await Aria2CClient.ChangeTellOption(pauseResult.Result, new Dictionary<string, object>() { { "select-file", numbers } }, _source.Token);
            var result = await Aria2CClient.Unpause(pauseResult.Result, _source.Token);
        }
        await refreshDownloads();
    }

    private async void _timer_Tick(object sender, object e)
    {
        await Refresh();
    }
}
