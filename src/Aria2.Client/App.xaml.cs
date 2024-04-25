using Aria2.Client.Common;
using Aria2.Client.Extentions;
using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Windows.ApplicationModel.Activation;

namespace Aria2.Client;

public sealed partial class App : ClientApplication
{
    public App()
    {
        this.InitializeComponent();
        this.UnhandledException += App_UnhandledException;
        
    }

    private void App_UnhandledException(
        object sender,
        Microsoft.UI.Xaml.UnhandledExceptionEventArgs e
    )
    {
        e.Handled = true;
    }

    /// <summary>
    /// PluginFolder
    /// </summary>
    public static string SearchPluginFolder =
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Aria2ClientPlugin";

    public Microsoft.Windows.AppLifecycle.AppInstance Instance { get; private set; }

    protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        Instance = AppInstance.FindOrRegisterForKey("Aria2.Client");
        Instance.Activated += Instance_Activated;
        var activatedEventArgs = AppInstance.GetCurrent().GetActivatedEventArgs();
        if (Instance.IsCurrent)
        {
            await ProgramLife.GetService<ILocalSettingsService>()!.InitSetting(new Dictionary<string,object>()
            {
                {AppSettingKey.ThemeColor, 1},
                {AppSettingKey.WallpaperEnable, true }
            });
            FileHelper.CheckFolder(SearchPluginFolder);
            var application = ProgramLife.GetService<IApplicationSetup<App>>();
            await application.LauncherAsync(this,activatedEventArgs);
        }
        else
        {
            await Instance.RedirectActivationToAsync(activatedEventArgs);
            Process.GetCurrentProcess().Kill();
        }
    }


    private void Instance_Activated(object sender, AppActivationArguments e)
    {
        if (e.Kind == ExtendedActivationKind.File)
        {
            var file = e.Data as FileActivatedEventArgs;
            var result = file.Files;
        }
    }
}