using Aria2.Client.Services.Contracts;
using Aria2.Net;
using Aria2.Net.Models.Enums;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

namespace Aria2.Client.ViewModels;

public sealed partial class OverviewViewModel : ObservableRecipient
{
    public OverviewViewModel(IAria2cClient aria2CClient,ITipShow tipShow)
    {
        Aria2CClient = aria2CClient;
        TipShow = tipShow;
    }

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
        var downloadResult = await this.Aria2CClient.ChangGlobalOption(Aria2GlobalOptionEnum.MaxAllDownloadLimit, $"{newValue}{SelectDownload}");
        if (oldValue == default) return;
        if (downloadResult.Result == GlobalUsings.RequestOK)
        {
            TipShow.ShowMessage("修改配置成功", Microsoft.UI.Xaml.Controls.Symbol.Accept);
        }
    }


    async partial void OnInputUploadChanged(double oldValue, double newValue)
    {
        var downloadResult = await this.Aria2CClient.ChangGlobalOption(Aria2GlobalOptionEnum.MaxAllDownloadLimit, $"{newValue}{SelectDownload}");
        if (oldValue == default) return;
        if (downloadResult.Result == GlobalUsings.RequestOK)
        {
            TipShow.ShowMessage("修改配置成功", Microsoft.UI.Xaml.Controls.Symbol.Accept);
        }
    }

    public IAria2cClient Aria2CClient { get; }
    public ITipShow TipShow { get; }
}
