using H.NotifyIcon;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Threading.Tasks;
using WinUIEx;

namespace Aria2.Client.Services.Contracts;

public interface IApplicationSetup<App>
    where App : Aria2.Client.Common.ClientApplication
{
    public App Application { get; }
    public Task LauncherAsync(App app, Microsoft.Windows.AppLifecycle.AppActivationArguments activatedEventArgs);

    public void TryEnqueue(Action action);


    public TaskbarIcon NotyfiIcon { get; }

    public bool IsSystemSetup { get; }

    public void SetSystemSetup(string appPath, bool enable);

    public AppActivationArguments LauncherArgs { get; }

    public void RegisterNotifyIcon(TaskbarIcon icon);

    public void ShowLeftPanelWindow();
}
