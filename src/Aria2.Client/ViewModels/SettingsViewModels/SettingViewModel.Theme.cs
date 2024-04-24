using Aria2.Client.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.UI.Controls.TextToolbarSymbols;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

namespace Aria2.Client.ViewModels;

partial class SettingViewModel
{
    [ObservableProperty]
    int _ThemeColor;

    partial void OnThemeColorChanged(int value)
    {
        LocalSettingsService.SaveConfig(AppSettingKey.ThemeColor,value);
        ThemeService.SetThemeAsync((ElementTheme)value);
    }
}