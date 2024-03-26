﻿using Aria2.Client.Common;
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
using System.Drawing;
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


    public DownloadTellItemData(IAria2cClient aria2CClient, IDataFactory dataFactory)
    {
        Aria2CClient = aria2CClient;
        DataFactory = dataFactory;
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
            if (this._stopFlage)
                return;
            var result = (await Aria2CClient.GetTellStatusAsync(this.Data.Gid, token));
            if (result == null) return;
            this.Data = result.Result;
            DataRefresh(this.Data);
            this.ProgressRatio = Math.Round(
                double.Parse(Data.CompletedLength) / double.Parse(Data.TotalLength) * 100,
                2
            );
            this.IsRemoved = (Data.Status == TellState.Stopped || Data.Status == TellState.Removed);
        }
        catch (Exception ex)
        {

        }
        
    }

    [RelayCommand]
    async Task OpenFolderLocal()
    {
        var result =  await this.Aria2CClient.GetFiles(this.Data.Gid);
        if (Data.Files.Count == 1)
        {
            int v = Shell.ShellExecute(
                IntPtr.Zero, // 父窗口句柄，NULL表示无父窗口
                "open", // 操作类型，这里为打开操作
                this.Data.Dir, // 文件名或路径，这里是要打开的文件夹路径
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
                System.IO.Path.GetDirectoryName(Data.Files[0].Path), // 文件名或路径，这里是要打开的文件夹路径
                null, // 参数，对于打开文件夹不需要参数
                null, // 初始目录，可以为null，默认当前目录
                ShowCommands.SW_SHOW
            ); // 显示方式，如SW_SHOWNORMAL
        }
        
        //Process.Start($"explorer.exe {this.Data.Dir}");
    }

    private void DataRefresh(FileDownloadTell data)
    {
        this._gid = data.Gid;
        RefreshFileName();
        switch (data.Status)
        {
            case TellState.Active:
                this.StateFont = "\uE769";
                break;
            case TellState.Waiting:
                this.StateFont = "\uE768";
                break;
            case TellState.Paused:
                this.StateFont = "\uE768";
                break;
            default:
                this.StateFont = "\uE783";
                break;
        }
    }

    [ObservableProperty]
    string _StateFont;

    [RelayCommand]
    async Task RemoveStopTask()
    {
        this._timer.Stop();
        bool clearFlage = false;
        if (
            this.Data.Status == TellState.Removed
            || this.Data.Status == TellState.Complete
            || this.Data.Status == TellState.Error
        )
        {
            var clear = await Aria2CClient.Aria2RemoveDownloadResult(this._gid);
            if(clear == null)
                WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveItemMessager>(new(this, true, false));
            if (clear.Result == GlobalUsings.RequestOK)
            {
                clearFlage = true;
                WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveItemMessager>(new(this, true, false));
            }
        }
        else
        {
            var result = await Aria2CClient.RemoveTask(this._gid, token);
            if (result == null)
                return;
            if (result.Result == this.Data.Gid)
            {
                var clear = await Aria2CClient.Aria2RemoveDownloadResult(this._gid);
                if(clear == null)
                {
                    WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveItemMessager>(new(this, true, false));
                }
                else if(clear.Result == GlobalUsings.RequestOK)
                {
                    clearFlage = true;
                    WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveItemMessager>(new(this, true, false));
                }
            }
        }
        if (!clearFlage)
            _timer.Start();
    }

    [RelayCommand]
    async Task ActiveTask()
    {
        if (this.Data.Status == TellState.Active)
        {
            var result = await Aria2CClient.PauseTask(this.Data.Gid, token);
            if (result.Result == this._gid)
            {
                WeakReferenceMessenger.Default.Send<TellTaskStateAddRemoveMessager>(
                    new(TellState.Active, TellState.Paused, _gid)
                );
            }
        }
        else if (
            this.Data.Status == TellState.Waiting
            || this.Data.Status == TellState.Paused
            || this.Data.Status == TellState.Removed
        )
        {
            var result = await Aria2CClient.Unpause(this.Data.Gid, token);
            if(result == null)return;
            if (result.Result == this._gid)
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
            this.FileName = System.IO.Path.GetFileName(Data.Files[0].Path);
            var icon =  FileHelper.GetIcon(Data.Files[0].Path, true);
            if(icon == null)
            {
                this.FileImageVisibility = true;
                return;
            }
            var bitmap = icon.ToBitmap();
            if (bitmap == null)
            {
                this.FileImageVisibility = true;
                return;
            }
            this.FileImage = await FileHelper.BitmapToBitmapImage(bitmap);
        }
        else
        {
            DirectoryInfo info = new DirectoryInfo(Data.Files[0].Path);
            this.FileName = info.Parent.Name;
            this.FileImage = await FileHelper.BitmapToBitmapImage(FileHelper.GetIcon(info.Parent.FullName, true).ToBitmap());
            this.FileImageVisibility = false;
        }
    }

    public override void MessageReceive(TellTaskSwitchMessager message)
    {
        if (message.OldStatus == message.NewStatus)
            return;
        if (message.NewStatus != this.Data.Status)
        {
            cts.Cancel();
            _timer.Stop();
        }
    }
}
