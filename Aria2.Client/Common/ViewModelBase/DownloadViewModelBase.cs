using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Aria2.Client.Common.ViewModelBase;

public abstract partial class DownloadViewModelBase: ObservableRecipient
{
    public DownloadViewModelBase(IAria2cClient aria2CClient, IDataFactory dataFactory)
    {
        Aria2CClient = aria2CClient;
        DataFactory = dataFactory;
        OnInitEnd();
    }

    [ObservableProperty]
    ObservableCollection<DownloadTellItemData> _Downloads = new();

    public IAria2cClient Aria2CClient { get; }
    public IDataFactory DataFactory { get; }


    public abstract void OnInitEnd();
}
