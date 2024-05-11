using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Aria2.Client.Helpers;
using Aria2.Client.Services.Contracts;
using Aria2.Client.UI.Controls;
using Aria2.Client.Views;
using Aria2.Client.Views.FirstLaunchView;
using Aria2.Client.Views.InstallerPluginView;
using Aria2.Client.Views.NotifyViews;
using Aria2.Net.Common;
using Aria2.Net.Services.Contracts;
using BtSearch.Loader;
using H.NotifyIcon;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.Win32;
using Microsoft.Windows.AppLifecycle;
using WinUIEx;

namespace Aria2.Client.Services;

public class ApplicationSetup<App> : IApplicationSetup<App>
    where App : Aria2.Client.Common.ClientApplication
{
    public ApplicationSetup(
        IThemeService<App> themeService,
        ILocalSettingsService localSettingsService
    )
    {
        LeftPane = null;
        ThemeService = themeService;
        LocalSettingsService = localSettingsService;
    }

    public App Application { get; private set; }

    public string AppName = "Aria2.Client";

    public TaskbarIcon NotyfiIcon { get; private set; }

    public WindowEx LeftPane;

    public bool IsSystemSetup
    {
        get
        {
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run",
                false
            );

            if (startupKey != null)
            {
                string currentValue = (string)startupKey.GetValue(AppName);

                if (currentValue != null)
                {
                    return true;
                }
            }
            startupKey?.Close();
            return false;
        }
    }

    public AppActivationArguments LauncherArgs { get; private set; }
    public IThemeService<App> ThemeService { get; }
    public ILocalSettingsService LocalSettingsService { get; }

    public async Task LauncherAsync(
        App app,
        Microsoft.Windows.AppLifecycle.AppActivationArguments activatedEventArgs
    )
    {
        this.LauncherArgs = activatedEventArgs;
        var isFlag = Convert.ToBoolean(await LocalSettingsService.ReadConfig("SetupFlag"));
        if (isFlag)
        {
            this.Application = app;
            this.Application.MainWindow = new();
            this.Application.MainWindow.SystemBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop();
            this.Application.MainWindow.ExtendsContentIntoTitleBar = true;
            Application.MainWindow.Content = ProgramLife.GetService<HelloAria2Page>();
            this.Application.MainWindow.MinWidth = 800;
            this.Application.MainWindow.MinHeight = 470;
            this.Application.MainWindow.Width = 800;
            this.Application.MainWindow.Height = 470;
            this.Application.MainWindow.MaxWidth = 800;
            this.Application.MainWindow.MaxHeight = 470;
            this.Application.MainWindow.IsMaximizable = false;
            this.Application.MainWindow.IsMinimizable = false;
            this.Application.MainWindow.IsResizable = true;
            this.Application.MainWindow.Activate();
        }
        else if (
            activatedEventArgs.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.Launch
        )
        {
            this.Application = app;
            this.Application.MainWindow = new();
            this.Application.MainWindow.ExtendsContentIntoTitleBar = true;
            Application.MainWindow.Content = ProgramLife.GetService<ShellPage>();
            this.Application.MainWindow.AppWindow.Closing += (sender, e) =>
            {
                e.Cancel = true;
                this.Application.MainWindow.Hide();
            };
            this.Application.MainWindow.SystemBackdrop = new MicaBackdrop();
            this.Application.MainWindow.Activate();
            this.Application.MainWindow.MinWidth = 500;
            this.Application.MainWindow.MinHeight = 550;
            var trackers = await ProgramLife
                .GetService<ILocalSettingsService>()
                .ReadObjectConfig<List<string>>("Trackers");
            var config = new Aria2LauncherConfig()
            {
                SesionFilePath = Aria2Config.SessionPath,
                LogFilePath = Aria2Config.LogPath,
                BtTracker = trackers,
                MaxDownloadSpeed = "0",
                MaxUploadSpeed = "0",
                MaxSaveResultCount = 20
            };
            InitSettings(config);
            await ProgramLife.GetService<IAria2cClient>().LauncherAsync(config);
            await ProgramLife.GetService<IAria2cClient>().ConnectAsync();
        }
        else if (
            activatedEventArgs.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.File
        )
        {
            this.Application = app;
            this.Application.MainWindow = new();
            this.Application.MainWindow.SystemBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop();
            this.Application.MainWindow.ExtendsContentIntoTitleBar = true;
            Application.MainWindow.Content = ProgramLife.GetService<PluginShellPage>();
            this.Application.MainWindow.Activate();
        }
        await InitAsync(app);
    }

    private async Task InitAsync(App app)
    {
        await ThemeService.InitializeAsync(app);
    }

    private void InitSettings(Aria2LauncherConfig config)
    {
        var localsetting = ProgramLife.GetService<ILocalSettingsService>();
        Dictionary<string, object> settings = new() { { "LauncherConfig", config } };
        localsetting.InitSetting(settings);
    }

    public void SetSystemSetup(string appPath, bool enable)
    {
        RegistryKey key = null;
        key = Registry.CurrentUser.OpenSubKey(
            @"Software\Microsoft\Windows\CurrentVersion\Run",
            true
        );

        if (key != null)
        {
            if (enable)
            {
                key.SetValue(AppName, appPath);
            }
            else
            {
                key.DeleteValue(AppName, false);
            }
        }
        key?.Close();
    }

    public void TryEnqueue(Action action)
    {
        this.Application.MainWindow.DispatcherQueue.TryEnqueue(
            new Microsoft.UI.Dispatching.DispatcherQueueHandler(() => action())
        );
    }

    public void RegisterNotifyIcon(TaskbarIcon icon)
    {
        this.NotyfiIcon = icon;
    }

    public void ShowLeftPanelWindow()
    {
        LeftPane = new();
        LeftPane.SystemBackdrop = new DesktopAcrylicBackdrop();
        LeftPane.Height = 450;
        LeftPane.Width = 350;
        var postion = WindowHelper.GetCursorPos(out var lpPoint);
        var width = WindowHelper.GetSystemMetrics(WindowHelper.SM_CXSCREEN);
        var height = WindowHelper.GetSystemMetrics(WindowHelper.SM_CYSCREEN);
        var warkspace = WindowHelper.GetDesktopWorkArea();
        var windowWidth = LeftPane.Width;
        var halfWindowWidth = windowWidth / 2;
        var desiredLeft = lpPoint.X - halfWindowWidth;
        var workspace = WindowHelper.GetDesktopWorkArea();
        if (desiredLeft < workspace.Left)
        {
            desiredLeft = workspace.Left;
        }
        else if (desiredLeft + windowWidth > workspace.Right)
        {
            desiredLeft = workspace.Right - windowWidth;
        }
        var desiredTop = workspace.Height;
        var dpi = TitleBar.GetScaleAdjustment(LeftPane);
        LeftPane.MoveAndResize(
            desiredLeft,
            desiredTop - (LeftPane.Height* dpi),
            windowWidth,
            LeftPane.Height
        );
        LeftPane.ExtendsContentIntoTitleBar = true;
        LeftPane.IsMinimizable = false;
        LeftPane.IsMaximizable = false;
        LeftPane.IsResizable = false;
        LeftPane.IsTitleBarVisible = false;
        LeftPane.Content = ProgramLife.GetService<NotyfiMainPage>();
        LeftPane.Activate();
        LeftPane.Activated += LeftPane_Activated;
        LeftPane.Content.Focus(FocusState.Pointer);
    }

    private void LeftPane_Activated(object sender, Microsoft.UI.Xaml.WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState == Microsoft.UI.Xaml.WindowActivationState.Deactivated)
        {
            Debug.WriteLine("取消激活");
            LeftPane.Close();
            LeftPane = null;
        }
        else if (
            args.WindowActivationState == Microsoft.UI.Xaml.WindowActivationState.CodeActivated
        )
        {
            ShowLeftPanelWindow();
        }
        else if (
            args.WindowActivationState == Microsoft.UI.Xaml.WindowActivationState.PointerActivated
        )
        {
            ShowLeftPanelWindow();
        }
    }
}
