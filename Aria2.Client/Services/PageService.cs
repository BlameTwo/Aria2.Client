using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels;
using Aria2.Client.ViewModels.DownloadViewModels;
using Aria2.Client.Views;
using Aria2.Client.Views.DownloadPages;
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
