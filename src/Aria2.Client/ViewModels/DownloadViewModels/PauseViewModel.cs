using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models.Messagers;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Linq;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.DownloadViewModels;

public sealed partial class PauseViewModel : DownloadViewModelBase, IRecipient<PauseDownloadStateMessager>,IRecipient<TellTaskStateAddRemoveItemMessager>
{
    public PauseViewModel(
        IAria2cClient aria2CClient,
        IDataFactory dataFactory,
        IApplicationSetup<App> applicationSetup
    )
        : base(aria2CClient, dataFactory, applicationSetup) { this.IsActive = true; }

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
        this.Downloads.Clear();
        var result = await Aria2CClient.GetWaitingTaskAsync(0,1000,TokenSource.Token);
        if (result.Result == null)
            return;
        foreach (var item in result.Result)
        {
            var download = DataFactory.CreateownloadTellItemData(item);
            AddDownload(download);
        }
    }

    public void Receive(PauseDownloadStateMessager message)
    {
        foreach (var item in Downloads.ToList())
        {
            if(item.Data.Gid == message.Gid)
            {
                if (message.IsRemove)
                    Downloads.Remove(item);
                else
                    Downloads.Add(item);
            }
        }
    }

    public void Receive(TellTaskStateAddRemoveItemMessager message)
    {
        if (message.IsRemove)
            this.Downloads.Remove(message.Value);
    }

    private void Aria2CClient_Aria2DownloadStateEvent(
        Net.Enums.WebSocketEventType eventType,
        Net.Models.WebSocketResultCode state
    )
    {
        ApplicationSetup.TryEnqueue(async () =>
        {
            await OnRefreshAsync();
        });
    }
}
