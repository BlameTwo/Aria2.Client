using Aria2.Client.Models;
using Aria2.Client.Models.Messagers;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.FirstLaunchViewModel;

public sealed partial class ThemeSettingViewModel : ObservableRecipient
{
    public ThemeSettingViewModel(
        [FromKeyedServices(ServiceKey.WelcomeNavigationServiceKey)]
            INavigationService navigationService,
        ILocalSettingsService localSettingsService,
        IThemeService<App> themeService
    )
    {
        NavigationService = navigationService;
        LocalSettingsService = localSettingsService;
        ThemeService = themeService;
    }

    public INavigationService NavigationService { get; }
    public ILocalSettingsService LocalSettingsService { get; }
    public IThemeService<App> ThemeService { get; }

    [ObservableProperty]
    int _ThemeColor;

    async partial void OnThemeColorChanged(int value)
    {
        await LocalSettingsService.SaveConfig(AppSettingKey.ThemeColor, value);
        await ThemeService.SetThemeAsync((ElementTheme)value);
    }

    [ObservableProperty]
    bool _WallpaperEnable;

    async partial void OnWallpaperEnableChanged(bool value)
    {
        await LocalSettingsService.SaveConfig(AppSettingKey.WallpaperEnable, value);
        WeakReferenceMessenger.Default.Send<AppWallpaperMessager>(new(value));
    }

    [RelayCommand]
    void Back()
    {
        NavigationService.GoBack();
    }

    [RelayCommand]
    async Task Launch()
    {
        await LocalSettingsService.SaveConfig("SetupFlag", false);
        AppInstance.Restart(null);
    }
}
