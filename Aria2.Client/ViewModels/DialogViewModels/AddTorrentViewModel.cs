﻿using Aria2.Client.Models.Messagers;
using Aria2.Client.Services.Contracts;
using Aria2.Net;
using Aria2.Net.Models.ClientModel;
using Aria2.Net.Services;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace Aria2.Client.ViewModels.DialogViewModels;

public sealed partial class AddTorrentViewModel:ObservableObject
{
    public AddTorrentViewModel(IAria2cClient aria2CClient,IApplicationSetup<App> applicationSetup,IDialogManager dialogManager)
    {
        Aria2CClient = aria2CClient;
        ApplicationSetup = applicationSetup;
        DialogManager = dialogManager;
    }

    [ObservableProperty]
    string _SaveFolder= "C:\\Users\\30140\\Downloads";

    [ObservableProperty]
    string _TorrentName = "选择Torrent文件";

    public IAria2cClient Aria2CClient { get; }
    public IApplicationSetup<App> ApplicationSetup { get; }
    public IDialogManager DialogManager { get; }

    [ObservableProperty]
    ObservableCollection<DownloadFile> _Downloads=new();
    private string _gid;

    [RelayCommand]
    async Task OpenTorrentFile()
    {
        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(ApplicationSetup.Application.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
        openPicker.FileTypeFilter.Add(".torrent");
        var file = await openPicker.PickSingleFileAsync();
        if (file == null) return;
        TorrentName = file.Path;
        var data = await Aria2CClient.AddTorrentAsync(TorrentName, new Dictionary<string, object>() { { "dir", SaveFolder } });
        var pauseResult = await Aria2CClient.PauseTask(data.Result);
        if (pauseResult.Result == data.Result)
        {
            var files = await Aria2CClient.GetFiles(data.Result);
            if (files != null)
            {
                this.Downloads = new(files.Result);
                this._gid = pauseResult.Result;
            }
        }
    }

    [RelayCommand]
    async Task DownloadFile()
    {
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
        var state = await Aria2CClient.GetTellStatusAsync(_gid);
        if (state.Result.Status == TellState.Paused)
        {
            var data = await Aria2CClient.ChangeTellOption(this._gid, new Dictionary<string, object>() { { "select-file", numbers } });
            var result = await Aria2CClient.Unpause(_gid);
            var files = await Aria2CClient.GetFiles(this._gid);
            DialogManager.CloseDialog();
        }
    }

    [RelayCommand]
    async Task Close()
    {
        try
        {
            if (Downloads.Count == 0) return;
            var state = await Aria2CClient.GetTellStatusAsync(_gid);
            if (state.Result.Status == TellState.Paused)
            {
                var removeId = await Aria2CClient.RemoveTask(this._gid);
                var stopState = await Aria2CClient.GetTellStatusAsync(_gid);
                if (removeId != null)
                {
                    await Aria2CClient.Aria2RemoveDownloadResult(removeId.Result);
                }
                WeakReferenceMessenger.Default.Send<PauseDownloadStateMessager>(new(true, this._gid));
            }
        }
        catch (Exception)
        {

        }
        finally
        {
            DialogManager.CloseDialog();
        }
    }
}
