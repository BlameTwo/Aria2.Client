using Aria2.Client.Services;
using Aria2.Client.Services.Contracts;
using Aria2.Client.Services.Navigations;
using Aria2.Client.Services.NavigationViews;
using Aria2.Client.ViewModels;
using Aria2.Client.ViewModels.DialogViewModels;
using Aria2.Client.Views;
using Aria2.Client.Views.Dialogs;
using Aria2.Net.Services;
using Aria2.Net.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Aria2.Client;

public static class ProgramLife
{
    static ProgramLife()
    {
        Service = new ServiceCollection()
            .AddSingleton<IApplicationSetup<App>,ApplicationSetup<App>>()
            .AddSingleton<IPageService,PageService>()
            .AddSingleton<IDialogManager,DialogManager>()
            .AddKeyedScoped<INavigationService,ShellNavigationService>(ServiceKey.ShellNavigationServiceKey)
            .AddKeyedScoped<INavigationService,HomeNavigationService>(ServiceKey.HomeNavigationServiceKey)
            .AddKeyedScoped<INavigationViewService,HomeNavigationViewService>(ServiceKey.HomeNavigationViewServiceKey)
            .AddSingleton<IAria2cClient,Aria2cClient>()
            .AddTransient<ShellPage>()
            .AddTransient<ShellViewModel>()
            .AddTransient<HomePage>()
            .AddTransient<HomeViewModel>()
            .AddTransient<AddUriDialog>()
            .AddTransient<AddUriViewModel>()
            .BuildServiceProvider();
    }

    private static ServiceProvider Service { get; }


    public static T GetService<T>()
        =>Service.GetService<T>();
}