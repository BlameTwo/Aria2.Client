using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels.DownloadViewModels;
using Aria2.Client.Views.DownloadPages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

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

    [RelayCommand]
    void Loaded()
    {
        NavigationService.NavigationTo<ActiveViewModel>(null);
    }

    private void NavigationService_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        var selectedItem = this.NavigationViewService.GetSelectItem(e.SourcePageType);
        if (selectedItem != null)
        {
            this.NavigationSelectItem = selectedItem;
        }

        if(e.SourcePageType == typeof(ActivePage))
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
    }

    [ObservableProperty]
    object _NavigationSelectItem;

    [ObservableProperty]
    string _Title;

    public INavigationViewService NavigationViewService { get; }
    public INavigationService NavigationService { get; }
}
