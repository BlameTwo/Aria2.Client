using Aria2.Net.Common;
using Aria2.Net.Models.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

partial class OverviewViewModel
{
    [ObservableProperty]
    int _MaxResult;

    async partial void OnMaxResultChanged(int oldValue, int newValue)
    {
        var downloadResult = await this.Aria2CClient.ChangGlobalOption(Aria2GlobalOptionEnum.MaxDownloadSaveResultCount,newValue);
        this.Config.MaxSaveResultCount = this.MaxResult;
        await LocalSettingsService.SaveConfig<Aria2LauncherConfig>("LauncherConfig", Config, Ctr.Token);
        if (oldValue == default) return;
        ShowResult(downloadResult.Result);
    }


    [RelayCommand]
    async Task RefreshMaxResult()
    {
        var option = await this.Aria2CClient.GetGlobalOption(Ctr.Token);
        this.MaxResult = int.Parse(option.Result.MaxDownloadResult);
    }
}
