using Aria2.Client.Models;
using Aria2.Client.Models.Messagers;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels.FrameViewModels;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

public sealed partial class ShellViewModel : ObservableRecipient, IRecipient<Tuple<bool, AppNotifyMessager>>,IRecipient<AppClearNotifyMessager>
{
    public ShellViewModel(
        IApplicationSetup<App> applicationSetup,
        [FromKeyedServices(ServiceKey.ShellNavigationServiceKey)]
            INavigationService navigationService,
        [FromKeyedServices(ServiceKey.HomeNavigationServiceKey)]
            INavigationService homeNavigationService,
        IDialogManager dialogManager,
        IAria2cClient aria2CClient,
        ITipShow tipShow,
        IDataFactory dataFactory,IAppMessageService appMessageService
    )
    {
        ApplicationSetup = applicationSetup;
        NavigationService = navigationService;
        HomeNavigationService = homeNavigationService;
        DialogManager = dialogManager;
        Aria2CClient = aria2CClient;
        TipShow = tipShow;
        DataFactory = dataFactory;
        AppMessageService = appMessageService;
        Aria2CClient.Aria2ConnectStateChanged += Aria2CClient_Aria2ConnectStateChanged;
        this.IsActive = true;
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
    public IDataFactory DataFactory { get; }
    public IAppMessageService AppMessageService { get; }

    private void Aria2CClient_Aria2ConnectStateChanged(
        object source,
        System.Net.WebSockets.WebSocketState state
    )
    {
        try
        {
            ApplicationSetup.Application.MainWindow.DispatcherQueue.TryEnqueue(async () =>
            {
                if (state == System.Net.WebSockets.WebSocketState.Open)
                {
                    ConnectState = new SolidColorBrush(Colors.Green);
                    ConnectText = "连接";
                    return;
                }
                if (ApplicationSetup.Application.MainWindow.DispatcherQueue == null)
                    return;
                ConnectState = new SolidColorBrush(Colors.Red);
                ConnectText = "断开";
                await Aria2CClient.ReconnectionSocket();
            });
        }
        catch (System.Exception) { }
    }

    [ObservableProperty]
    ObservableCollection<AppMessageItemData> _MessageList=new();

    [ObservableProperty]
    int _MessageCount=0;

    partial void OnMessageCountChanged(int value)
    {
        if (value == 0)
            MessageInfoVisibility = Visibility.Collapsed;
        else
            MessageInfoVisibility = Visibility.Visible;
    }

    [ObservableProperty]
    Visibility _MessageInfoVisibility = Visibility.Collapsed;

    [RelayCommand]
    void Loaded()
    {
        NavigationService.NavigationTo<HomeViewModel>(null);
        AppMessageService.SendMessage("Aria2启动成功", "应用消息", Models.Enums.MessageLevel.Default,true);
        AppMessageService.SendTimeSpanMessage(TimeSpan.FromSeconds(60), "延时关闭消息", "应用消息", Models.Enums.MessageLevel.Default);
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

    [RelayCommand]
    void ClearMessage()
    {
        foreach (var item in MessageList.ToArray())
        {
            if(item.Data.IsClear)
                MessageList.Remove(item);
        }
        MessageCount = MessageList.Count;
    }

    public void Receive(Tuple<bool, AppNotifyMessager> message)
    {
        if (message.Item1)
        {
            this.MessageList.Add(
                DataFactory.CreateItemData<AppMessageItemData, AppNotifyMessager>(message.Item2)
            );
        }
        else
        {
            foreach (var item in this.MessageList.ToList())
            {
                if(item.Data == message.Item2 )
                {
                    MessageList.Remove(item);
                    break;
                }
            }
        }

        this.MessageCount = MessageList.Count;
    }

    public void Receive(AppClearNotifyMessager message)
    {
        foreach (var item in this.MessageList.ToArray())
        {
            if(item.Data.Guid == message.Guid)
            {
                MessageList.Remove(item);
            }
        }
        MessageCount = MessageList.Count;
    }
}
