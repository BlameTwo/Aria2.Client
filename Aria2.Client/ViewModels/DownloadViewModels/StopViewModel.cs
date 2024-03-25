using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.DownloadViewModels;

public sealed partial class StopViewModel : DownloadViewModelBase
{
    public StopViewModel(
        IAria2cClient aria2CClient,
        IDataFactory dataFactory,
        IApplicationSetup<App> applicationSetup
    )
        : base(aria2CClient, dataFactory, applicationSetup) { }

    public override void OnInitEnd()
    {
        this.Aria2CClient.Aria2DownloadStateEvent += Aria2CClient_Aria2DownloadStateEvent;
    }

    public async override Task OnRefreshAsync()
    {
        var result = await Aria2CClient.GetStopedTaskAsync();
        if (result.Result == null)
            return;
        foreach (var item in result.Result)
        {
            var download = DataFactory.CreateownloadTellItemData(item);
            AddDownload(download);
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
