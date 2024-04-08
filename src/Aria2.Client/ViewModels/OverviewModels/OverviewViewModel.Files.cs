using Aria2.Client.Extentions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;

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
        var folder = System.IO.Path.GetDirectoryName(LogPath);
        folder.CheckFolder();
        await Launcher.LaunchFolderPathAsync(folder);
    }


    [RelayCommand]
    async Task SelectSessionPath() 
    {
        var folder = System.IO.Path.GetDirectoryName(SessionPath);
        folder.CheckFolder();
        await Launcher.LaunchFolderPathAsync(folder);
    }
}
