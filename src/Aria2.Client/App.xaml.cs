﻿using Aria2.Client.Common;
using Aria2.Client.Extentions;
using Aria2.Client.Helpers;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Diagnostics;

namespace Aria2.Client;

public sealed partial class App : ClientApplication
{
    public App()
    {
        this.InitializeComponent();
        this.UnhandledException += App_UnhandledException;
        
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        e.Handled = true;
    }

    

    public static string SearchPluginFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Aria2ClientPlugin";

    public Microsoft.Windows.AppLifecycle.AppInstance Instance { get; private set; }

    protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        Instance = AppInstance.FindOrRegisterForKey("Aria2.Client");
        Instance.Activated += Instance_Activated;
        if (Instance.IsCurrent)
        {
            FileHelper.CheckFolder(SearchPluginFolder);
            var application = ProgramLife.GetService<IApplicationSetup<App>>();
            application.Launcher(this);
            await ProgramLife.GetService<IAria2cClient>().LauncherAsync(new()
            {
                SesionFilePath = "D:\\save.session",
                BtTracker = new()
            {
                "http://93.158.213.92:1337/announce",
                "udp://23.137.251.46:6969/announce",
                "udp://23.134.90.6:1337/announce",
                "udp://185.243.218.213:80/announce",
                "udp://91.216.110.53:451/announce",
                "udp://208.83.20.20:6969/announce",
                "udp://107.189.11.58:6969/announce",
                "udp://222.216.138.162:6969/announce",
                "udp://109.201.134.183:80/announce",
                "udp://198.100.149.66:6969/announce",
                "udp://23.157.120.14:6969/announce",
                "udp://83.146.98.78:6969/announce"
            }
            });
            await ProgramLife.GetService<IAria2cClient>().ConnectAsync();
        }
        else
        {
            var eventargs = Microsoft.Windows.AppLifecycle.AppInstance
            .GetCurrent()
                .GetActivatedEventArgs();
            await Instance.RedirectActivationToAsync(eventargs);
            Process.GetCurrentProcess().Kill();
        }
        
    }

    private void Instance_Activated(object sender, AppActivationArguments e)
    {
        if(e.Kind == ExtendedActivationKind.StartupTask)
        {
            
        }
    }
}

