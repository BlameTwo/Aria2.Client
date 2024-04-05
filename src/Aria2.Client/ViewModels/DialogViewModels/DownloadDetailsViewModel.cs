using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using Aria2.Net;
using Aria2.Net.Models;
using Aria2.Net.Models.ClientModel;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.DialogViewModels;

public sealed partial class DownloadDetailsViewModel : ObservableRecipient
{
    CancellationTokenSource _source = new CancellationTokenSource();

    public DownloadDetailsViewModel(
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
    ObservableCollection<BittorrentPeer> _Peers;

    [ObservableProperty]
    int _SelectFile;

    [ObservableProperty]
    int _Size;

    [RelayCommand]
    async Task RefreshDownloads() => await refreshDownloads();

    [RelayCommand]
    void CloseDialog()
    {
        this._timer.Stop();
        this._timer = null;
        this.Peers = null;
        this.Data = null;
        this._source.Cancel();
        this.DialogManager.CloseDialog();
    }

    async Task Refresh()
    {
        if (Data != null)
        {
            var ips = await Aria2CClient.GetBittorrentPeers(this.Gid, _source.Token);
            this.Peers = new(ips.Result);
        }
    }

    partial void OnTabSelectoritemChanged(SelectorBarItem value)
    {
        if (value.Text == "任务详情")
        {
            TaskVisibility = Visibility.Visible;
            LinkVisibility = Visibility.Collapsed;
            DataVisibility = Visibility.Collapsed;
        }
        else if(value.Text == "连接")
        {
            TaskVisibility = Visibility.Collapsed;
            LinkVisibility = Visibility.Visible;
            DataVisibility = Visibility.Collapsed;
        }
        else if(value.Text == "Data")
        {

            TaskVisibility = Visibility.Collapsed;
            LinkVisibility = Visibility.Collapsed;
            DataVisibility = Visibility.Visible;
        }
    }

    public async Task RefreshTask(string gidName)
    {
        this.Gid = gidName;
        var data = await Aria2CClient.GetTellStatusAsync(this.Gid, _source.Token);
        this.Data = DataFactory.CreateownloadTellItemData(data.Result);
        _timer = new DispatcherTimer();
        _timer.Tick += _timer_Tick;
        _timer.Interval = new(0, 0, 1);
        _timer.Start();
    }

    private async Task refreshDownloads()
    {
        if (this.Data.Data.Bittorrent == null)
            return;
        var downloads = await this.Aria2CClient.GetFiles(this.Gid,_source.Token);
        if (downloads == null)
            return;
        this.Downloads = new(downloads.Result);
        this.SelectFile = Downloads.Count();
        this.Size = 2312;
    }

    [RelayCommand]
    async Task EditerTask()
    {
        var pauseResult = await Aria2CClient.ForcePaush(Gid, _source.Token);
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
