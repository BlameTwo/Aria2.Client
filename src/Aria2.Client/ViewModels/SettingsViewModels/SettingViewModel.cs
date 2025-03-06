using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using System;

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

        ThemeColor = ((int)await LocalSettingsService.ReadConfig(AppSettingKey.ThemeColor));
        WallpaperEnable = Convert.ToBoolean((await LocalSettingsService.ReadConfig(AppSettingKey.WallpaperEnable)));
        #endregion
    }

    public ILocalSettingsService LocalSettingsService { get; }
    public IThemeService<App> ThemeService { get; }
}
