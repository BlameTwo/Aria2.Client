using Aria2.Client.Services.Contracts;
using Aria2.Client.Views;

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
        this.Application.MainWindow.Activate();
    }
}
