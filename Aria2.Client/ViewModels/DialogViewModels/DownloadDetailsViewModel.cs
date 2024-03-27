using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Aria2.Client.ViewModels.DialogViewModels;

public sealed partial class DownloadDetailsViewModel:ObservableRecipient
{
    public DownloadDetailsViewModel(IAria2cClient aria2CClient)
    {
        Aria2CClient = aria2CClient;
    }

    public IAria2cClient Aria2CClient { get; }

    public void RefreshTask(string gidName)
    {

    }
}
