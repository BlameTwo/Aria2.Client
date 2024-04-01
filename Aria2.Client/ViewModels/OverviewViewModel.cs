using Aria2.Net.Models.Enums;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

namespace Aria2.Client.ViewModels;

public sealed partial class OverviewViewModel : ObservableRecipient
{
    public OverviewViewModel(IAria2cClient aria2CClient)
    {
        Aria2CClient = aria2CClient;
    }

    [ObservableProperty]
    List<string> _MaxUpload = new() { "50K", "100K", "5M", "20M", };

    [ObservableProperty]
    List<string> _MaxDownload = new() { "500K", "2M", "10M", "60M", };

    [ObservableProperty]
    string _SelectUpload;

    [ObservableProperty]
    string _SelectDownload;

    async partial void OnSelectDownloadChanged(string value)
    {
       var downloadResult = await this.Aria2CClient.ChangGlobalOption(Aria2GlobalOptionEnum.MaxAllDownloadLimit, value);
    }

    async partial void OnSelectUploadChanged(string value)
    {
        var uploadResult = await this.Aria2CClient.ChangGlobalOption(Aria2GlobalOptionEnum.MaxAllUploadLimit, value);
    }

    public IAria2cClient Aria2CClient { get; }
}
