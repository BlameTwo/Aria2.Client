using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;

namespace Aria2.Client.ViewModels.DownloadViewModels;

public sealed partial class ActiveViewModel : DownloadViewModelBase
{
    public IApplicationSetup<App> ApplicationSetup { get; }

    public ActiveViewModel(IAria2cClient aria2CClient, IDataFactory dataFactory, IApplicationSetup<App> applicationSetup)
        :base(aria2CClient, dataFactory)
    {
        ApplicationSetup = applicationSetup;
    }


    private void Aria2CClient_DownloadStartEvent(
        object source,
        Net.Models.WebSocketResultCode result
    )

    {
        ApplicationSetup.TryEnqueue(async() =>
        {
            if (result.Params == null || result.Params.Count == 0)
                return;
            foreach (var item in result.Params)
            {
                var gid = await Aria2CClient.GetTellStatusAsync(item.Gid);
                if (gid.Result == null)
                    continue;
                var tell = DataFactory.CreateownloadTellItemData(gid.Result);
                Downloads.Add(tell);
            }
        });
    }

    private void Aria2CClient_Aria2WebSocketMessage(
        object source,
        Net.Models.WebSocketResultCode result
    ) { }

    public override void OnInitEnd()
    {
        Aria2CClient.DownloadStartEvent += Aria2CClient_DownloadStartEvent;
        Aria2CClient.Aria2WebSocketMessage += Aria2CClient_Aria2WebSocketMessage;
        Aria2CClient.DownloadCompleteEvent += Aria2CClient_DownloadCompleteEvent;
    }

    private void Aria2CClient_DownloadCompleteEvent(object source, Net.Models.WebSocketResultCode result)
    {
        ApplicationSetup.TryEnqueue(() =>
        {
            foreach (var item in Downloads)
            {
                foreach (var gid in result.Params)
                {
                    if (item.Data.Gid == gid.Gid)
                        Downloads.Remove(item);
                }
            }
        });
    }
}
