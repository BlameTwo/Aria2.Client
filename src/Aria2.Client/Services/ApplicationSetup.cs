using Aria2.Client.Services.Contracts;
using Aria2.Client.Views;
using Aria2.Net.Services.Contracts;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace Aria2.Client.Services;

public class ApplicationSetup<App> : IApplicationSetup<App>
    where App:Aria2.Client.Common.ClientApplication
{
    public App Application { get; private set; }

    public string AppName = "Aria2.Client";

    public bool IsSystemSetup
    {
        get
        {
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);

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

    public void Launcher(App app)
    {
        this.Application = app;
        this.Application.MainWindow = new();
        this.Application.MainWindow.SystemBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop();
        this.Application.MainWindow.ExtendsContentIntoTitleBar = true;
        Application.MainWindow.Content = ProgramLife.GetService<ShellPage>();
        Application.MainWindow.AppWindow.Closing += async (sender, e) =>
        {
            await ProgramLife.GetService<IAria2cClient>().ExitAria2();
        };
        this.Application.MainWindow.Activate();
    }

    public void SetSystemSetup(string appPath, bool enable)
    {
        RegistryKey key = null;
        key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

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
          this.Application.MainWindow.DispatcherQueue.TryEnqueue(new Microsoft.UI.Dispatching.DispatcherQueueHandler(()=> action()));
    }

}
