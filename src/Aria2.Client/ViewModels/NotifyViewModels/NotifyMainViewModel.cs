using Aria2.Client.Common;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Models.ClientModel;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WinUIEx;

namespace Aria2.Client.ViewModels.NotifyViewModels;

public sealed partial class NotifyMainViewModel:ObservableRecipient
{
    public NotifyMainViewModel(IAria2cClient aria2CClient,IApplicationSetup<App> applicationSetup)
    {
        Aria2CClient = aria2CClient;
        ApplicationSetup = applicationSetup;
    }


    public IAria2cClient Aria2CClient { get; }
    public IApplicationSetup<App> ApplicationSetup { get; }

    
    [RelayCommand]
    async Task Exit()
    {
        await Aria2CClient.DisconnectAsync();
        await Aria2CClient.ExitAria2();
        Process.GetCurrentProcess().Kill();
    }

    [RelayCommand]
    void Show()
    {
        if (!ApplicationSetup.Application.MainWindow.AppWindow.IsVisible)
        {
            ApplicationSetup.Application.MainWindow.Activate();
        }
        else
        {
            ApplicationSetup.Application.MainWindow.Hide();
        }
    }
}
