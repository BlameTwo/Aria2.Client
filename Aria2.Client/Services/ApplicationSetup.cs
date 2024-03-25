using Aria2.Client.Services.Contracts;
using Aria2.Client.Views;
using Aria2.Net.Services.Contracts;
using System;

namespace Aria2.Client.Services;

public class ApplicationSetup<App> : IApplicationSetup<App>
    where App:Aria2.Client.Common.ClientApplication
{
    public App Application { get; private set; }

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

    public void TryEnqueue(Action action)
        => this.Application.MainWindow.DispatcherQueue.TryEnqueue(() => action());
}
