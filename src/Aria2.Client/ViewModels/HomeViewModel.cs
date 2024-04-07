using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels.DownloadViewModels;
using Aria2.Client.ViewModels.FrameViewModels;
using Aria2.Client.Views;
using Aria2.Client.Views.DownloadPages;
using Aria2.Client.Views.FramePages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IBtSearch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

public sealed partial class HomeViewModel : ObservableObject
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
        
    }

    [ObservableProperty]
    object _NavigationSelectItem;

    [ObservableProperty]
    string _Title;

    public INavigationViewService NavigationViewService { get; }
    public INavigationService NavigationService { get; }

    [RelayCommand]
    async Task Loaded()
    {
        NavigationService.NavigationTo<ActiveViewModel>(null);
    }

    private void NavigationService_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
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
        if(e.SourcePageType == typeof(OverviewPage))
        {
            this.Title = "总览";
        }
        if(e.SourcePageType == typeof(AnimePage))
        {
            this.Title = "Anime Search";
        }
        if(e.SourcePageType == typeof(SearchPage))
        {
            this.Title = "搜索种子";
        }
        if(e.SourcePageType == typeof(PluginPage))
        {
            this.Title = "插件管理";
        }
    }

}
