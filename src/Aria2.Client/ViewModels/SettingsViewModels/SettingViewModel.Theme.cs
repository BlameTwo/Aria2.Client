using Aria2.Client.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace Aria2.Client.ViewModels;

partial class SettingViewModel
{
    [ObservableProperty]
    int _ThemeColor;

    async partial void OnThemeColorChanged(int value)
    {
        await LocalSettingsService.SaveConfig(AppSettingKey.ThemeColor,value);
        await ThemeService.SetThemeAsync((ElementTheme)value);
    }
}