using Aria2.Client.Models.Bases;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.Input;
using IBtSearch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Aria2.Client.Models;

public sealed partial class BTSearchRresultItem : ItemDownloadBase<BTSearchResult>
{
    public BTSearchRresultItem(IAria2cClient aria2CClient)
    {
        Aria2CClient = aria2CClient;
    }

    public IAria2cClient Aria2CClient { get; }

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
    }
}
