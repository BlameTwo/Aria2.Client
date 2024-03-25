using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models.Messagers;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Linq;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.DownloadViewModels;

public sealed partial class PauseViewModel : DownloadViewModelBase, IRecipient<PauseDownloadStateMessager>
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

    public async override Task OnRefreshAsync()
    {
        var result = await Aria2CClient.GetWaitingTaskAsync(0,1,TokenSource.Token);
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

    private void Aria2CClient_Aria2DownloadStateEvent(
        Net.Enums.WebSocketEventType eventType,
        Net.Models.WebSocketResultCode state
    )
    {
        if (eventType == Net.Enums.WebSocketEventType.Pause)
        {
            this.AddDownload(state.Params);
            return;
        }
        this.RemoveDownload(state.Params);
    }
}
