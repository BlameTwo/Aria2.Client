using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models.Messagers;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
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
        Aria2CClient.Aria2DownloadStateEvent += Aria2CClient_Aria2DownloadStateEvent;
    }

    public override void Unregister()
    {
        foreach (var item in Downloads)
        {
            item.Disponse();
        }
        Downloads.Clear();
        Aria2CClient.Aria2DownloadStateEvent -= Aria2CClient_Aria2DownloadStateEvent;
        base.Unregister();
    }
    public override async Task OnRefreshAsync()
    {
        var result = await Aria2CClient.GetStoppedTaskAsync(0, 1000, TokenSource.Token);
        if (result.Result == null)
            return;
        foreach (var download in result.Result.Select(DataFactory.CreateownloadTellItemData))
        {
            AddDownload(download);
        }
    }

    public void Receive(TellTaskStateAddRemoveItemMessager message)
    {
        if (message.IsRemove)
        {
            foreach (var item in Downloads.ToList().Where(item => item.Data.Gid == message.Value.Data.Gid))
            {
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
            eventType is Net.Enums.WebSocketEventType.Stop or Net.Enums.WebSocketEventType.Error
        )
        {
            AddDownload(state.Params);
            return;
        }
        RemoveDownload(state.Params);
    }
}
