using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using System;
using System.Text.Json;

namespace Aria2.Client.ViewModels;

public partial class SettingViewModel : PageViewModelBase
{
    public SettingViewModel(ILocalSettingsService localSettingsService,IThemeService<App> themeService) : base("设置")
    {
        ThemeColor = 0;
        LocalSettingsService = localSettingsService;
        ThemeService = themeService;
        Init();
    }

    private async void Init()
    {
        #region themeColor

        this.ThemeColor = ((int)await LocalSettingsService.ReadConfig(AppSettingKey.ThemeColor));
        this.WallpaperEnable = Convert.ToBoolean((await LocalSettingsService.ReadConfig(AppSettingKey.WallpaperEnable)));
        #endregion
    }

    public ILocalSettingsService LocalSettingsService { get; }
    public IThemeService<App> ThemeService { get; }
}
