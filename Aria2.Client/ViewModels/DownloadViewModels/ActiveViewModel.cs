using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models;
using Aria2.Client.Models.Messagers;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.DownloadViewModels;

public sealed partial class ActiveViewModel : DownloadViewModelBase,IRecipient<TellTaskStateAddRemoveItemMessager>
{
    public ActiveViewModel(
        IAria2cClient aria2CClient,
        IDataFactory dataFactory,
        IApplicationSetup<App> applicationSetup
    )
        : base(aria2CClient, dataFactory, applicationSetup) { IsActive = true; }

    private void Aria2CClient_Aria2WebSocketMessage(
        object source,
        Net.Models.WebSocketResultCode result
    ) { }

    public override void OnInitEnd()
    {
        Aria2CClient.Aria2WebSocketMessage += Aria2CClient_Aria2WebSocketMessage;
        Aria2CClient.Aria2DownloadStateEvent += Aria2CClient_Aria2DownloadStateEvent;
    }

    private void Aria2CClient_Aria2DownloadStateEvent(
        Net.Enums.WebSocketEventType eventType,
        Net.Models.WebSocketResultCode state
    )
    {
        if (eventType == Net.Enums.WebSocketEventType.Start)
        {
            AddDownload(state.Params);
        }
        else
        {
            RemoveDownload(state.Params);
        }
    }

    public override void OnUnLoad()
    {
        base.OnUnLoad();
    }

    public async override Task OnRefreshAsync()
    {
        var result = await Aria2CClient.GetAllTellActiveAsync();
        if (result == null || result.Result == null)
            return;
        foreach (var item in result.Result)
        {
            var download = DataFactory.CreateownloadTellItemData(item);
            AddDownload(download);
        }
    }

    public void Receive(TellTaskStateAddRemoveItemMessager message)
    {
        if (message.IsRemove)
        {
            foreach (var item in Downloads.ToList())
            {
                if(item.Data.Gid == message.Value.Data.Gid)
                    Downloads.Remove(item);
            }
        }
    }
}
