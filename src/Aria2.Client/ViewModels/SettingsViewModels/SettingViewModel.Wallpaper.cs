using Aria2.Client.Models;
using Aria2.Client.Models.Messagers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Aria2.Client.ViewModels;

partial class SettingViewModel
{
    [ObservableProperty]
    bool _WallpaperEnable;

    async partial void OnWallpaperEnableChanged(bool value)
    {
        await LocalSettingsService.SaveConfig(AppSettingKey.WallpaperEnable,value);
        WeakReferenceMessenger.Default.Send<AppWallpaperMessager>(new(value));
    }
}