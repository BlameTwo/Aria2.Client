using System;
using System.Collections.ObjectModel;
using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.Views.FirstLaunchView;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.ViewModels.FirstLaunchViewModel;

public sealed partial class HelloAria2ViewModel : ObservableRecipient
{
    public HelloAria2ViewModel(
        [FromKeyedServices(ServiceKey.WelcomeNavigationServiceKey)]
            INavigationService navigationService
        ,IApplicationSetup<App> applicationSetup
    )
    {
        NavigationService = navigationService;
        ApplicationSetup = applicationSetup;
        NavigationService.Navigated += NavigationService_Navigated;
    }

    private void NavigationService_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        if(e.Content.GetType() == typeof(FileSettingsPage))
        {
            NavigateHeader = new() { "文件设置" };
        }
        else if(e.Content.GetType() == typeof(ThemeSettingPage))
        {
            NavigateHeader = new() { "文件设置","主题设置" };
        }
    }

    [ObservableProperty]
    ObservableCollection<string> _NavigateHeader = new() { "文件设置" };

    

    public INavigationService NavigationService { get; }
    public IApplicationSetup<App> ApplicationSetup { get; }

    [RelayCommand]
    void Loaded()
    {
        this.NavigationService.NavigationTo<FileSettingsViewModel>(null);
    }

    internal void RefreshContent(BreadcrumbBarItemClickedEventArgs args)
    {
        if (args.Index == 0)
        {
            this.NavigationService.NavigationTo<FileSettingsViewModel>(null);
        }
        else if (args.Index == 1)
        {
            this.NavigationService.NavigationTo<ThemeSettingViewModel>(null);
        }
    }
}
