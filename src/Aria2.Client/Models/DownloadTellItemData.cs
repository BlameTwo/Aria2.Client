using Aria2.Client.Extentions;
using Aria2.Client.Helpers;
using Aria2.Client.Models.Bases;
using Aria2.Client.Models.Messagers;
using Aria2.Client.Services.Contracts;
using Aria2.Net;
using Aria2.Net.Models.ClientModel;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static Aria2.Client.Common.Shell;

namespace Aria2.Client.Models;

public partial class DownloadTellItemData : ItemDownloadBase<FileDownloadTell>
{
    private DispatcherTimer _timer = new();
    CancellationTokenSource cts = null;
    CancellationToken token = default;
    private string _gid = string.Empty;
    private bool _stopFlage = false;

    [ObservableProperty]
    string _FileName;

    [ObservableProperty]
    double _ProgressRatio;


    [ObservableProperty]
    bool _IsRemoved;


    [ObservableProperty]
    BitmapImage _FileImage;

    [ObservableProperty]
    bool _FileImageVisibility;

    [ObservableProperty]
    string _FileIcon;

    public IAria2cClient Aria2CClient { get; }

    public IDataFactory DataFactory { get; }

    public IDialogManager DialogManager { get; }

    public DownloadTellItemData(IAria2cClient aria2CClient, IDataFactory dataFactory,IDialogManager dialogManager)
    {
        Aria2CClient = aria2CClient;
        DataFactory = dataFactory;
        DialogManager = dialogManager;
        _timer.Interval = TimeSpan.FromSeconds(1);
        cts = new CancellationTokenSource();
        token = cts.Token;
        _timer.Tick += _timer_Tick;
        _timer.Start();
    }


    private async void _timer_Tick(object sender, object e)
    {
        try
        {
            if (_stopFlage)
                return;
            var result = (await Aria2CClient.GetTellStatusAsync(Data.Gid, token));
            if (result == null) return;
            Data = result.Result;
            if (Data == null) return;
            DataRefresh(Data);
            ProgressRatio = Math.Round(
                double.Parse(Data.CompletedLength) / double.Parse(Data.TotalLength) * 100,
                2
            );
            IsRemoved = (Data.Status == TellState.Stopped || Data.Status == TellState.Removed);
        }
        catch (Exception ex)
        {

        }
        
    }

    [RelayCommand]
    async Task OpenFolderLocal()
    {
        var result =  await Aria2CClient.GetFiles(Data.Gid);
        if (Data.Files.Count == 1)
        {
            int v = ShellExecute(
                IntPtr.Zero, // 父窗口句柄，NULL表示无父窗口
                "open", // 操作类型，这里为打开操作
                Data.Dir, // 文件名或路径，这里是要打开的文件夹路径
                null, // 参数，对于打开文件夹不需要参数
                null, // 初始目录，可以为null，默认当前目录
                ShowCommands.SW_SHOW
            ); // 显示方式，如SW_SHOWNORMAL
        }
        else
        {
            int v = ShellExecute(
                IntPtr.Zero, // 父窗口句柄，NULL表示无父窗口
                "open", // 操作类型，这里为打开操作
                Path.GetDirectoryName(Data.Files[0].Path), // 文件名或路径，这里是要打开的文件夹路径
                null, // 参数，对于打开文件夹不需要参数
                null, // 初始目录，可以为null，默认当前目录
                ShowCommands.SW_SHOW
            ); // 显示方式，如SW_SHOWNORMAL
        }
        
        //Process.Start($"explorer.exe {this.Data.Dir}");
    }

    private async void DataRefresh(FileDownloadTell data)
    {
        _gid = data.Gid;
        RefreshFileName();
        if(Data.Bittorrent != null)
        {
            var result = await Aria2CClient.GetBittorrentPeers(_gid);
            if(Data.Bitfield != null)
            {
                var value = TellHelper.GetBitfield(Data.Bitfield.ToUpper());
                Bitfields = value;
            }
        }
        switch (data.Status)
        {
            case TellState.Active:
                StateFont = "\uE769";
                break;
            case TellState.Waiting:
                StateFont = "\uE768";
                break;
            case TellState.Paused:
                StateFont = "\uE768";
                break;
            default:
                StateFont = "\uE783";
                break;
        }
    }

    [ObservableProperty]
    string _StateFont;

    [ObservableProperty]
    ObservableCollection<double> _Bitfields=new();

    [RelayCommand]
    async Task RemoveStopTask()
    {
        _timer.Stop();
        string gid = "";
        bool clearFlage = false;
        var result = await Aria2CClient.RemoveTask(_gid, token);
        if (result == null)
        {
            WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveItemMessager>(new(this, true, false));
            return;
        }
        gid = result.Result;
        while (true)
        {
            await Task.Delay(300, token);
            if (token.IsCancellationRequested)
                break;
            var getResult = await Aria2CClient.GetTellStatusAsync(gid, token);
            if(getResult == null)
            {
                clearFlage = true;
                WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveItemMessager>(new(this, true, false));
                break;
            }
            if (getResult.Result.Status == TellState.Stopped || getResult.Result.Status == TellState.Paused || getResult.Result.Status== TellState.Removed)
            {
                gid = getResult.Result.Gid;
                break;
            }
        }
        if (token.IsCancellationRequested)
            return;
        var clear = await Aria2CClient.Aria2RemoveDownloadResult(gid);
        if(clear == null)
        {
            clearFlage = true;
            WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveItemMessager>(new(this, true, false));
        }
        else if (clear.Result == GlobalUsings.RequestOK)
        {
            clearFlage = true;
            WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveItemMessager>(new(this, true, false));
        }
        if(!clearFlage)
            _timer.Start();
    }

    [RelayCommand]
    async Task ActiveTask()
    {
        if (Data.Status == TellState.Active)
        {
            var result = await Aria2CClient.ForcePause(Data.Gid, token);
            if (result == null)
            {
                WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveMessager>(
                    new(TellState.Active, TellState.Paused, _gid)
                );
                return;
            }
            if (result.Result == _gid)
            {
                WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveMessager>(
                    new(TellState.Active, TellState.Paused, _gid)
                );
            }
        }
        else if (
            Data.Status == TellState.Waiting
            || Data.Status == TellState.Paused
            || Data.Status == TellState.Removed
        )
        {
            var result = await Aria2CClient.Unpause(Data.Gid, token);
            if(result == null)return;
            if (result.Result == _gid)
            {
                WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveMessager>(
                    new(TellState.Paused, TellState.Active, _gid)
                );
            }
        }
        else
        {
            // MessageBox.Show("任务终止！");
        }
    }

    public override void SetData(FileDownloadTell data)
    {
        base.SetData(data);
        DataRefresh(data);
        
    }

    private async void RefreshFileName()
    {
        if (Data.Files.Count == 1)
        {
            FileName = Path.GetFileName(Data.Files[0].Path);
            var icon =  FileHelper.GetIcon(Data.Files[0].Path, true);
            if(icon == null)
            {
                FileImageVisibility = true;
                return;
            }
            var bitmap = icon.ToBitmap();
            if (bitmap == null)
            {
                FileImageVisibility = true;
                return;
            }
            FileImage = await FileHelper.BitmapToBitmapImage(bitmap);
        }
        else
        {
            DirectoryInfo info = new DirectoryInfo(Data.Files[0].Path);
            FileName = info.Parent.Name;
            FileImage = await FileHelper.BitmapToBitmapImage(FileHelper.GetIcon(info.Parent.FullName, true).ToBitmap());
            FileImageVisibility = false;
        }
    }

    public override void MessageReceive(TellTaskSwitchMessager message)
    {
        if (message.OldStatus == message.NewStatus)
            return;
        if (message.NewStatus != Data.Status)
        {
            cts.Cancel();
            _timer.Stop();
        }
    }

    public void Disponse()
    {
        cts.Cancel();
        
        _timer.Stop();
    }

    [RelayCommand]
    void OpenDetailsDialog()
    {
        //DialogManager.ShowDownloadDetails(this.Data.Gid);
        WeakReferenceMessenger.Default.Send<OpenDownloadSessionMessager>(new(true, Data.Gid));
    }
}
