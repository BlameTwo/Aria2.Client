using Aria2.Net.Models.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

partial class OverviewViewModel
{
    [ObservableProperty]
    public partial int MaxResult { get; set; }

    async partial void OnMaxResultChanged(int oldValue, int newValue)
    {
        var downloadResult =
            await Aria2CClient.ChangeGlobalOption(Aria2GlobalOptionEnum.MaxDownloadSaveResultCount, newValue.ToString());
        Config.MaxSaveResultCount = MaxResult;
        await LocalSettingsService.SaveConfig("LauncherConfig", Config, Ctr.Token);
        if (oldValue == 0) return;
        ShowResult(downloadResult.Result);
    }


    [RelayCommand]
    async Task RefreshMaxResult()
    {
        var option = await Aria2CClient.GetGlobalOption(Ctr.Token);
        MaxResult = int.Parse(option.Result.MaxDownloadResult);
    }
}
