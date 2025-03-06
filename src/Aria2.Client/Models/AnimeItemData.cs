using Aria2.Client.Models.Anime;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace Aria2.Client.Models;

public sealed partial class AnimeItemData : ObservableRecipient,IItemData<AnimeResource>,IDisposable
{
    [ObservableProperty]
    AnimeResource _Data;

    public IAria2cClient Aria2CClient { get; }
    public ITipShow TipShow { get; }
    public IDialogManager DialogManager { get; }

    public AnimeItemData(IAria2cClient aria2CClient,ITipShow tipShow,IDialogManager dialogManager)
    {
        Aria2CClient = aria2CClient;
        TipShow = tipShow;
        DialogManager = dialogManager;
    }

    public void SetData(AnimeResource data)
    {
        Data = data;
        DataRefresh();
    }

    private void DataRefresh()
    {

    }


    [RelayCommand]
    async Task AddDownload()
    {

        await DialogManager.ShowAddUriAsync(Data.Magnet);
        //var result = await Aria2CClient.AddUriAsync(
        //    new List<string>() { this.Data.Magnet },
        //    new Dictionary<string, object>()
        //    {
        //        { "dir", UserDataPaths.GetDefault().Downloads },
        //        { "follow-torrent", true }
        //    },
        //    1
        //);
        //if (result != null)
        //{
        //    TipShow.ShowMessage($"创建任务：{result.Result}", Microsoft.UI.Xaml.Controls.Symbol.Accept);
        //}
        //else
        //{
        //    TipShow.ShowMessage($"创建失败!", Microsoft.UI.Xaml.Controls.Symbol.Clear);
        //}
    }

    public void Dispose()
    {
        Data = null;
        GC.Collect();
    }
}
