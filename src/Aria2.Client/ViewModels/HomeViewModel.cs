using Aria2.Client.Models.Messagers;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels.DownloadViewModels;
using Aria2.Client.ViewModels.FrameViewModels;
using Aria2.Client.ViewModels.SplitViewModels;
using Aria2.Client.Views;
using Aria2.Client.Views.DownloadPages;
using Aria2.Client.Views.FramePages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IBtSearch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

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
        this.TellDownloadSessionViewModel = ProgramLife.GetService<TellDownloadSessionViewModel>();
        this.IsActive = true;
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
        var selectedItem = this.NavigationViewService.GetSelectItem(e.SourcePageType);
        this.NavigationSelectItem = selectedItem;
        if (e.SourcePageType == typeof(ActivePage))
        {
            this.Title = "活动中";
        }
        if (e.SourcePageType == typeof(PausePage))
        {
            this.Title = "暂停中";
        }
        if (e.SourcePageType == typeof(StopPage))
        {
            this.Title = "停止中";
        }
        if (e.SourcePageType == typeof(OverviewPage))
        {
            this.Title = "总览";
        }
        if (e.SourcePageType == typeof(AnimePage))
        {
            this.Title = "Anime Search";
        }
        if (e.SourcePageType == typeof(SearchPage))
        {
            this.Title = "搜索种子";
        }
        if (e.SourcePageType == typeof(PluginPage))
        {
            this.Title = "插件管理";
        }
        if(e.SourcePageType == typeof(AboutPage))
        {
            this.Title = "关于";
        }
    }

    public async void Receive(OpenDownloadSessionMessager message)
    {
        this.IsPaneOpen = true;
        await this.TellDownloadSessionViewModel.RefreshTask(message.Gid);
    }
}
