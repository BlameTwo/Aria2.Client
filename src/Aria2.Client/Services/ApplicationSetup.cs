using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Aria2.Client.Common;
using Aria2.Client.Helpers;
using Aria2.Client.Services.Contracts;
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
using TitleBar = Aria2.Client.UI.Controls.TitleBar;

namespace Aria2.Client.Services;

public class ApplicationSetup<TApp>(
    IThemeService<TApp> themeService,
    ILocalSettingsService localSettingsService)
    : IApplicationSetup<TApp>
    where TApp : ClientApplication
{
    public TApp Application { get; private set; }

    public string AppName = "Aria2.Client";

    public TaskbarIcon NotyfiIcon { get; private set; }

    public WindowEx LeftPane = null;

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
    public IThemeService<TApp> ThemeService { get; } = themeService;
    public ILocalSettingsService LocalSettingsService { get; } = localSettingsService;

    public async Task LauncherAsync(
        TApp app,
        AppActivationArguments activatedEventArgs
    )
    {
        LauncherArgs = activatedEventArgs;
        var isFlag = Convert.ToBoolean(await LocalSettingsService.ReadConfig("SetupFlag"));
        if (isFlag)
        {
            Application = app;
            Application.MainWindow = new()
            {
                SystemBackdrop = new MicaBackdrop(),
                ExtendsContentIntoTitleBar = true,
                Content = ProgramLife.GetService<HelloAria2Page>(),
                MinWidth = 800,
                MinHeight = 470,
                Width = 800,
                Height = 470,
                MaxWidth = 800,
                MaxHeight = 470,
                IsMaximizable = false,
                IsMinimizable = false,
                IsResizable = true
            };
            Application.MainWindow.Activate();
        }
        else if (
            activatedEventArgs.Kind == ExtendedActivationKind.Launch
        )
        {
            Application = app;
            Application.MainWindow = new()
            {
                ExtendsContentIntoTitleBar = true,
                Content = ProgramLife.GetService<ShellPage>(),
                SystemBackdrop = new MicaBackdrop(),
                MinWidth = 500,
                MinHeight = 550
            };
            Application.MainWindow.AppWindow.Closing += (sender, e) =>
            {
                e.Cancel = true;
                Application.MainWindow.Hide();
            };
            Application.MainWindow.Activate();
            var trackers = await ProgramLife
                .GetService<ILocalSettingsService>()
                .ReadObjectConfig<List<string>>("Trackers");
            var config = new Aria2LauncherConfig()
            {
                SessionFilePath = Aria2Config.SessionPath,
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
            activatedEventArgs.Kind == ExtendedActivationKind.File
        )
        {
            Application = app;
            Application.MainWindow = new()
            {
                SystemBackdrop = new MicaBackdrop(),
                ExtendsContentIntoTitleBar = true,
                Content = ProgramLife.GetService<PluginShellPage>()
            };
            Application.MainWindow.Activate();
        }
        await InitAsync(app);
    }

    private async Task InitAsync(TApp app)
    {
        await ThemeService.InitializeAsync(app);
    }

    private void InitSettings(Aria2LauncherConfig config)
    {
        var localSetting = ProgramLife.GetService<ILocalSettingsService>();
        Dictionary<string, object> settings = new() { { "LauncherConfig", config } };
        localSetting.InitSetting(settings);
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
        Application.MainWindow.DispatcherQueue.TryEnqueue(
            new Microsoft.UI.Dispatching.DispatcherQueueHandler(() => action())
        );
    }

    public void RegisterNotifyIcon(TaskbarIcon icon)
    {
        NotyfiIcon = icon;
    }

    public void ShowLeftPanelWindow()
    {
        var position = WindowHelper.GetCursorPos(out var lpPoint);
        var width = WindowHelper.GetSystemMetrics(WindowHelper.SM_CXSCREEN);
        var height = WindowHelper.GetSystemMetrics(WindowHelper.SM_CYSCREEN);
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
        LeftPane = new()
        {
            SystemBackdrop = new DesktopAcrylicBackdrop(),
            Height = 450,
            Width = 350,
            ExtendsContentIntoTitleBar = true,
            IsMinimizable = false,
            IsMaximizable = false,
            IsResizable = false,
            IsTitleBarVisible = false,
            Content = ProgramLife.GetService<NotifyMainPage>()
        };
        LeftPane.MoveAndResize(
            desiredLeft,
            desiredTop - (LeftPane.Height* dpi),
            windowWidth,
            LeftPane.Height
        );
        LeftPane.Activate();
        LeftPane.Activated += LeftPane_Activated;
        LeftPane.Content.Focus(FocusState.Pointer);
    }

    private void LeftPane_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState == WindowActivationState.Deactivated)
        {
            Debug.WriteLine("取消激活");
            LeftPane.Close();
            LeftPane = null;
        }
        else if (
            args.WindowActivationState == WindowActivationState.CodeActivated
        )
        {
            ShowLeftPanelWindow();
        }
        else if (
            args.WindowActivationState == WindowActivationState.PointerActivated
        )
        {
            ShowLeftPanelWindow();
        }
    }
}
