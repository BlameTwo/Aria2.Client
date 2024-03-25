using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
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
        IDialogManager dialogManager,
        IAria2cClient aria2CClient
    )
    {
        ApplicationSetup = applicationSetup;
        NavigationService = navigationService;
        DialogManager = dialogManager;
        Aria2CClient = aria2CClient;
        Aria2CClient.Aria2ConnectStateChanged += Aria2CClient_Aria2ConnectStateChanged;
    }

    private void Aria2CClient_Aria2ConnectStateChanged(
        object source,
        System.Net.WebSockets.WebSocketState state
    )
    {
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

    [ObservableProperty]
    SolidColorBrush _ConnectState;

    [ObservableProperty]
    string _ConnectText;

    public IApplicationSetup<App> ApplicationSetup { get; }
    public INavigationService NavigationService { get; }
    public IDialogManager DialogManager { get; }
    public IAria2cClient Aria2CClient { get; }

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
}
