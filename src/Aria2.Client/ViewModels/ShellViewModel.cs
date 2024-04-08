﻿using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels.FrameViewModels;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

public sealed partial class ShellViewModel : ObservableObject
{
    public ShellViewModel(
        IApplicationSetup<App> applicationSetup,
        [FromKeyedServices(ServiceKey.ShellNavigationServiceKey)]
        INavigationService navigationService,
        [FromKeyedServices(ServiceKey.HomeNavigationServiceKey)]
        INavigationService homeNavigationService,
        IDialogManager dialogManager,
        IAria2cClient aria2CClient,
        ITipShow tipShow
    )
    {
        ApplicationSetup = applicationSetup;
        NavigationService = navigationService;
        HomeNavigationService = homeNavigationService;
        DialogManager = dialogManager;
        Aria2CClient = aria2CClient;
        TipShow = tipShow;
        Aria2CClient.Aria2ConnectStateChanged += Aria2CClient_Aria2ConnectStateChanged;
    }

    private void Aria2CClient_Aria2ConnectStateChanged(
        object source,
        System.Net.WebSockets.WebSocketState state
    )
    {
        try
        {
            if (ApplicationSetup.Application.MainWindow.DispatcherQueue == null) return;
            ApplicationSetup.Application.MainWindow.DispatcherQueue.TryEnqueue(async () =>
            {
                if (state == System.Net.WebSockets.WebSocketState.Open)
                {
                    ConnectState = new SolidColorBrush(Colors.Green);
                    ConnectText = "连接";
                    return;
                }
                ConnectState = new SolidColorBrush(Colors.Red);
                ConnectText = "断开";
                await Aria2CClient.ReconnectionSocket();
            });
        }
        catch (System.Exception)
        {
        }
    }

    [ObservableProperty]
    SolidColorBrush _ConnectState;

    [ObservableProperty]
    string _ConnectText;

    public IApplicationSetup<App> ApplicationSetup { get; }
    public INavigationService NavigationService { get; }
    public INavigationService HomeNavigationService { get; }
    public IDialogManager DialogManager { get; }
    public IAria2cClient Aria2CClient { get; }
    public ITipShow TipShow { get; }

    [RelayCommand]
    void Loaded()
    {
        NavigationService.NavigationTo<HomeViewModel>(null);
    }


    [RelayCommand]
    async Task ShowAddUri()
    {
        await DialogManager.ShowAddUriAsync();
    }

    [RelayCommand]
    async Task ShowAddTorrent()
    {
        await DialogManager.ShowAddTorrentAsync();
    }

    [RelayCommand]
    void GoAnimePage()
    {
        HomeNavigationService.NavigationTo<AnimeViewModel>(null);
    }

    [RelayCommand]
    void GoSearchPage()
    {
        HomeNavigationService.NavigationTo<SearchViewModel>(null);
    }
}