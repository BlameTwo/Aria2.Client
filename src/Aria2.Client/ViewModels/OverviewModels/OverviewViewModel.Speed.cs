﻿using Aria2.Net.Models.Enums;
using Aria2.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Aria2.Net.Common;

namespace Aria2.Client.ViewModels;

public partial class OverviewViewModel
{
    [ObservableProperty]
    List<string> _SpiltSource = new() { "K", "M", "G", };


    [ObservableProperty]
    string _SelectUpload;

    [ObservableProperty]
    string _SelectDownload;

    [ObservableProperty]
    double _InputDownload;

    [ObservableProperty]
    double _InputUpload;

    async partial void OnInputDownloadChanged(double oldValue, double newValue)
    {
        var downloadResult = await this.Aria2CClient.ChangGlobalOption(Aria2GlobalOptionEnum.MaxAllUploadLimit, $"{newValue}{SelectDownload}");
        this.Config.MaxDownloadSpeed = $"{newValue}{SelectDownload}";
        await LocalSettingsService.SaveConfig<Aria2LauncherConfig>("LauncherConfig", Config);
        if (oldValue == default) return;
        ShowResult(downloadResult.Result);
    }


    async partial void OnInputUploadChanged(double oldValue, double newValue)
    {
        var downloadResult = await this.Aria2CClient.ChangGlobalOption(Aria2GlobalOptionEnum.MaxAllDownloadLimit, $"{newValue}{SelectUpload}");
        this.Config.MaxUploadSpeed = $"{newValue}{SelectUpload}";
        await LocalSettingsService.SaveConfig<Aria2LauncherConfig>("LauncherConfig", Config);
        if (oldValue == default) return;
        ShowResult(downloadResult.Result);
    }


    [RelayCommand]
    async Task ClearSpeed()
    {
        var uoloadResult = await this.Aria2CClient.ChangGlobalOption(Aria2GlobalOptionEnum.MaxAllUploadLimit, $"0");
        var downloadResult = await this.Aria2CClient.ChangGlobalOption(Aria2GlobalOptionEnum.MaxAllDownloadLimit, $"0");
        if (uoloadResult.Result == GlobalUsings.RequestOK)
        {
            TipShow.ShowMessage("重置上传限制", Microsoft.UI.Xaml.Controls.Symbol.Accept);
            this.InputUpload = 0;
        }
        if (downloadResult.Result == GlobalUsings.RequestOK)
        {
            TipShow.ShowMessage("重置下载限制", Microsoft.UI.Xaml.Controls.Symbol.Accept);
            this.InputDownload = 0;
        }
    }
}
