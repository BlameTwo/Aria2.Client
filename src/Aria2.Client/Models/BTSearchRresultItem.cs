using Aria2.Client.Models.Bases;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.Input;
using IBtSearch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Aria2.Client.Models;

public sealed partial class BTSearchRresultItem : ItemDownloadBase<BTSearchResult>
{
    public BTSearchRresultItem(IAria2cClient aria2CClient,ITipShow tipShow,IAppCenterService appCenterService)
    {
        Aria2CClient = aria2CClient;
        TipShow = tipShow;
        AppCenterService = appCenterService;
    }

    public IAria2cClient Aria2CClient { get; }
    public ITipShow TipShow { get; }
    public IAppCenterService AppCenterService { get; }

    [RelayCommand]
    async Task Download()
    {
        var result = await Aria2CClient.AddUriAsync(
            new List<string>() { this.Data.BTUrl },
            new Dictionary<string, object>()
            {
                { "dir", UserDataPaths.GetDefault().Downloads },
                { "follow-torrent", true }
            },
            1
        );
        if(result != null)
        {
            TipShow.ShowMessage($"创建任务：{result.Result}",Microsoft.UI.Xaml.Controls.Symbol.Accept);
        }
        AppCenterService.SendAppCenter(new("Aria2.AddUrlDownload", new()
        {
            { "State",result!= null? "true": "false" }
        }));
    }
}
