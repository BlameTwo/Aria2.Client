using Aria2.Client.Models.Messagers;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels.DownloadViewModels;
using Aria2.Client.ViewModels.SplitViewModels;
using Aria2.Client.Views;
using Aria2.Client.Views.DownloadPages;
using Aria2.Client.Views.FramePages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Aria2.Client.ViewModels;

public sealed partial class HomeViewModel
    : ObservableRecipient,
        IRecipient<OpenDownloadSessionMessager>
{
    public HomeViewModel(
        [FromKeyedServices(ServiceKey.HomeNavigationViewServiceKey)]
            INavigationViewService navigationViewService,
        [FromKeyedServices(ServiceKey.HomeNavigationServiceKey)]
            INavigationService navigationService
    )
    {
        NavigationViewService = navigationViewService;
        NavigationService = navigationService;
        NavigationService.Navigated += NavigationService_Navigated;
        TellDownloadSessionViewModel = ProgramLife.GetService<TellDownloadSessionViewModel>();
        IsActive = true;
    }

    [ObservableProperty]
    object _NavigationSelectItem;

    [ObservableProperty]
    string _Title;

    [ObservableProperty]
    bool _IsPaneOpen;

    public INavigationViewService NavigationViewService { get; }
    public INavigationService NavigationService { get; }
    public TellDownloadSessionViewModel TellDownloadSessionViewModel { get; }

    [RelayCommand]
    void Loaded()
    {
        NavigationService.NavigationTo<ActiveViewModel>(null);
    }

    private void NavigationService_Navigated(
        object sender,
        Microsoft.UI.Xaml.Navigation.NavigationEventArgs e
    )
    {
        var selectedItem = NavigationViewService.GetSelectItem(e.SourcePageType);
        NavigationSelectItem = selectedItem;
        if (e.SourcePageType == typeof(ActivePage))
        {
            Title = "活动中";
        }
        if (e.SourcePageType == typeof(PausePage))
        {
            Title = "暂停中";
        }
        if (e.SourcePageType == typeof(StopPage))
        {
            Title = "停止中";
        }
        if (e.SourcePageType == typeof(OverviewPage))
        {
            Title = "总览";
        }
        if (e.SourcePageType == typeof(AnimePage))
        {
            Title = "Anime Search";
        }
        if (e.SourcePageType == typeof(SearchPage))
        {
            Title = "搜索种子";
        }
        if (e.SourcePageType == typeof(PluginPage))
        {
            Title = "插件管理";
        }
        if(e.SourcePageType == typeof(AboutPage))
        {
            Title = "关于";
        }
    }

    public async void Receive(OpenDownloadSessionMessager message)
    {
        IsPaneOpen = true;
        await TellDownloadSessionViewModel.RefreshTask(message.Gid);
    }
}
