using Aria2.Client.Models;
using Aria2.Client.Models.Messagers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

partial class SettingViewModel
{
    [ObservableProperty]
    bool _WallpaperEnable;

    partial void OnWallpaperEnableChanged(bool value)
    {
        LocalSettingsService.SaveConfig(AppSettingKey.WallpaperEnable,value);
        WeakReferenceMessenger.Default.Send<AppWallpaperMessager>(new(value));
    }
}