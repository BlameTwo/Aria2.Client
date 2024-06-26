﻿using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels;
using Aria2.Client.ViewModels.DownloadViewModels;
using Aria2.Client.ViewModels.FirstLaunchViewModel;
using Aria2.Client.ViewModels.FrameViewModels;
using Aria2.Client.Views;
using Aria2.Client.Views.DownloadPages;
using Aria2.Client.Views.FirstLaunchView;
using Aria2.Client.Views.FramePages;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace Aria2.Client.Services;

public sealed partial class PageService:IPageService
{
    private readonly Dictionary<string, Tuple<Type, Type>> _pages;

    public PageService()
    {
        _pages = new();
        RegisterView<HomePage, HomeViewModel>();
        RegisterView<ActivePage, ActiveViewModel>();
        RegisterView<PausePage, PauseViewModel>();
        RegisterView<StopPage, StopViewModel>();
        RegisterView<OverviewPage, OverviewViewModel>();
        RegisterView<AnimePage, AnimeViewModel>();
        RegisterView<SearchPage,SearchViewModel>();
        RegisterView<PluginPage, PluginViewModel>();
        RegisterView<AboutPage,AboutViewModel>();
        RegisterView<SettingPage,SettingViewModel>();


        #region 首次启动
        RegisterView<HelloAria2Page, HelloAria2ViewModel>();
        RegisterView<FileSettingsPage, FileSettingsViewModel>();
        RegisterView<ThemeSettingPage, ThemeSettingViewModel>();
        #endregion
    }

    public Type GetPage(string key)
    {
        _pages.TryGetValue(key, out var page);
        if(page is null)
        {
            return null;
        }
        return page.Item1;
    }

    public void RegisterView<View, ViewModel>()
        where View : Page
        where ViewModel : ObservableObject =>
        _pages.Add(typeof(ViewModel).FullName, new(typeof(View), typeof(ViewModel)));
}
