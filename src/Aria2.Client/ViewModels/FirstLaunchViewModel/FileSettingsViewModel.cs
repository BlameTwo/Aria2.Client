using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using BtSearch.Loader;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Aria2.Client.ViewModels.FirstLaunchViewModel;

public sealed partial class FileSettingsViewModel : ObservableRecipient
{
    public FileSettingsViewModel(
        [FromKeyedServices(ServiceKey.WelcomeNavigationServiceKey)]
            INavigationService navigationService,ILocalSettingsService localSettingsService
    )
    {
        NavigationService = navigationService;
        LocalSettingsService = localSettingsService;
        this.LogPath = Aria2Config.LogPath;
        this.SessionPath = Aria2Config.SessionPath;
    }

    public INavigationService NavigationService { get; }
    public ILocalSettingsService LocalSettingsService { get; }

    [ObservableProperty]
    string _LogPath;

    [ObservableProperty]
    string _SessionPath;

    [RelayCommand]
    void GoNext() 
    {
        this.NavigationService.NavigationTo<ThemeSettingViewModel>(null);
    }


}
