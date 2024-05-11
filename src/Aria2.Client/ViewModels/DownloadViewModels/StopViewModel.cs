using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models.Messagers;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Linq;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.DownloadViewModels;

public sealed partial class StopViewModel : DownloadViewModelBase, IRecipient<TellTaskStateAddRemoveItemMessager>
{
    public StopViewModel(
        IAria2cClient aria2CClient,
        IDataFactory dataFactory,
        IApplicationSetup<App> applicationSetup
    )
        : base(aria2CClient, dataFactory, applicationSetup) { IsActive = true; }

    public override void OnInitEnd()
    {
        this.Aria2CClient.Aria2DownloadStateEvent += Aria2CClient_Aria2DownloadStateEvent;
    }

    public override void Unregister()
    {
        foreach (var item in Downloads)
        {
            item.Disponse();
        }
        Downloads.Clear();
        this.Aria2CClient.Aria2DownloadStateEvent -= Aria2CClient_Aria2DownloadStateEvent;
        base.Unregister();
    }
    public async override Task OnRefreshAsync()
    {
        var result = await Aria2CClient.GetStopedTaskAsync(0,1000,TokenSource.Token);
        if (result.Result == null)
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
                if (item.Data.Gid == message.Value.Data.Gid)
                    Downloads.Remove(item);
            }
        }
    }

    private void Aria2CClient_Aria2DownloadStateEvent(
        Net.Enums.WebSocketEventType eventType,
        Net.Models.WebSocketResultCode state
    )
    {
        if (
            eventType == Net.Enums.WebSocketEventType.Stop
            || eventType == Net.Enums.WebSocketEventType.Error
        )
        {
            this.AddDownload(state.Params);
            return;
        }
        this.RemoveDownload(state.Params);
    }
}
