﻿using Aria2.Client.Models;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.Services.Navigations;
using Aria2.Client.Services.NavigationViews;
using Aria2.Client.ViewModels;
using Aria2.Client.ViewModels.DialogViewModels;
using Aria2.Client.ViewModels.DownloadViewModels;
using Aria2.Client.Views;
using Aria2.Client.Views.Dialogs;
using Aria2.Client.Views.DownloadPages;
using Aria2.Net.Contracts;
using Aria2.Net.Services;
using Aria2.Net.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Aria2.Client;

public static class ProgramLife
{
    static ProgramLife()
    {
        Service = new ServiceCollection()
            .AddSingleton<IApplicationSetup<App>, ApplicationSetup<App>>()
            .AddSingleton<IPageService, PageService>()
            .AddSingleton<IDialogManager, DialogManager>()
            .AddSingleton<IAria2cClient, Aria2cClient>()
            .AddSingleton<IAria2cOptionService, Aria2cOptionService>()
            .AddKeyedScoped<INavigationService, ShellNavigationService>(ServiceKey.ShellNavigationServiceKey)
            .AddKeyedScoped<INavigationService, HomeNavigationService>(ServiceKey.HomeNavigationServiceKey)
            .AddKeyedScoped<INavigationViewService, HomeNavigationViewService>(ServiceKey.HomeNavigationViewServiceKey)
            .AddSingleton<IAria2cClient, Aria2cClient>()
            .AddTransient<IDataFactory, DataFactory>()
            .AddTransient<ShellPage>()
            .AddTransient<ShellViewModel>()
            .AddTransient<HomePage>()
            .AddTransient<HomeViewModel>()
            .AddTransient<AddUriDialog>()
            .AddTransient<AddUriViewModel>()
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
        #endregion
            .BuildServiceProvider();
    }

    private static ServiceProvider Service { get; }


    public static T GetService<T>()
        =>Service.GetService<T>();
}