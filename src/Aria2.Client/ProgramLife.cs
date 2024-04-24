using Aria2.Client.Models;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.Services.Navigations;
using Aria2.Client.Services.NavigationViews;
using Aria2.Client.ViewModels;
using Aria2.Client.ViewModels.DialogViewModels;
using Aria2.Client.ViewModels.DownloadViewModels;
using Aria2.Client.ViewModels.FrameViewModels;
using Aria2.Client.ViewModels.InstallerPluginViewModel;
using Aria2.Client.ViewModels.NotifyViewModels;
using Aria2.Client.ViewModels.SplitViewModels;
using Aria2.Client.Views;
using Aria2.Client.Views.Dialogs;
using Aria2.Client.Views.DownloadPages;
using Aria2.Client.Views.FramePages;
using Aria2.Client.Views.InstallerPluginView;
using Aria2.Client.Views.NotifyViews;
using Aria2.Net.Contracts;
using Aria2.Net.Services;
using Aria2.Net.Services.Contracts;
using BtSearch.Loader.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace Aria2.Client;

public static class ProgramLife
{
    static ProgramLife()
    {
        Service = new ServiceCollection()
            .AddSingleton<IApplicationSetup<App>, ApplicationSetup<App>>()
            .AddSingleton<IThemeService<App>, ThemeService<App>>()
            .AddSingleton<IPageService, PageService>()
            .AddSingleton<IDialogManager, DialogManager>()
            .AddSingleton<IAria2cClient, Aria2cClient>()
            .AddSingleton<IPluginManager, PluginManager>()
            .AddSingleton<IAppMessageService, AppMessageService>()
            .AddSingleton<IAria2cOptionService, Aria2cOptionService>()
            .AddKeyedScoped<INavigationService, ShellNavigationService>(ServiceKey.ShellNavigationServiceKey)
            .AddKeyedScoped<INavigationService, HomeNavigationService>(ServiceKey.HomeNavigationServiceKey)
            .AddKeyedScoped<INavigationViewService, HomeNavigationViewService>(ServiceKey.HomeNavigationViewServiceKey)
            .AddSingleton<ITipShow, TipShow>()
            .AddSingleton<IAria2cClient, Aria2cClient>()
            .AddSingleton<IWallpaperService, WallpaperService>()
            .AddSingleton<IPickersService, PickersService>()
            .AddSingleton<ILocalSettingsService, LocalSettingsService>()
            .AddTransient<IDataFactory, DataFactory>()
            .AddSingleton<IOnekumaService, OnekumaService>()
            .AddTransient<ShellPage>()
            .AddTransient<ShellViewModel>()
            .AddTransient<SearchPage>()
            .AddTransient<SearchViewModel>()
            .AddTransient<HomePage>()
            .AddTransient<HomeViewModel>()
            .AddTransient<AddUriDialog>()
            .AddTransient<AddUriViewModel>()
            .AddTransient<SettingPage>()
            .AddTransient<SettingViewModel>()
            .AddTransient<OverviewPage>()
            .AddTransient<OverviewViewModel>()
            .AddTransient<AddTorrentDialog>()
            .AddTransient<AboutPage>()
            .AddTransient<AboutViewModel>()
            .AddTransient<AddTorrentViewModel>()
            .AddTransient<PluginViewModel>()
        #region 托盘图标
            .AddSingleton<NotyfiMainPage>()
            .AddSingleton<NotifyMainViewModel>()
        #endregion
        #region 插件安装
            .AddTransient<PluginShellPage>()
            .AddTransient<PluginShellViewModel>()
        #endregion
        #region 展开栏目内容
            .AddTransient<TellDownloadSessionViewModel>()
        #endregion
        #region 注册下载页面
            .AddTransient<ActivePage>()
            .AddTransient<ActiveViewModel>()
            .AddTransient<PausePage>()
            .AddTransient<PauseViewModel>()
            .AddTransient<StopPage>()
            .AddTransient<StopViewModel>()
        #endregion
        #region 注册子项
            .AddTransient<DownloadTellItemData>()
            .AddTransient<AnimeItemData>()
            .AddTransient<BTSearchRresultItem>()
            .AddTransient<BTPluginItemData>()
            .AddTransient<AppMessageItemData>()
        #endregion
        #region Anime
            .AddTransient<AnimePage>()
            .AddTransient<AnimeViewModel>()
        #endregion
            .BuildServiceProvider();
    }

    private static ServiceProvider Service { get; }


    public static T GetService<T>()
        =>Service.GetRequiredService<T>();
}