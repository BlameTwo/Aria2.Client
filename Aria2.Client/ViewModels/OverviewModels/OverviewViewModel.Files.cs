using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

partial class OverviewViewModel
{
    [ObservableProperty]
    string _LogPath;


    [ObservableProperty]
    string _SessionPath;


    [RelayCommand]
    async Task SelectLogPath()
    {
        var logpath = await PickersService.GetFolderPicker();
        if (logpath == null) return;
        this.LogPath = logpath.Path+"aria2.log"; 
        var result =  await Aria2CClient.ChangGlobalOption(Net.Models.Enums.Aria2GlobalOptionEnum.LogFilePath, SessionPath);
        ShowResult(result.Result);
    }


    [RelayCommand]
    async Task SelectSessionPath() 
    {
        var logpath = await PickersService.GetFolderPicker();
        if (logpath == null) return;
        this.SessionPath = logpath.Path+"save.session";
        var result =  await Aria2CClient.ChangGlobalOption(Net.Models.Enums.Aria2GlobalOptionEnum.SaveSession, SessionPath);
        ShowResult(result.Result);
    }
}
