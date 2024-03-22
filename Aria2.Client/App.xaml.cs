using Aria2.Client.Common;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace Aria2.Client;

public sealed partial class App : ClientApplication
{
    public App()
    {
        this.InitializeComponent();
    }

    protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        ProgramLife.GetService<IApplicationSetup<App>>().Launcher(this);
        await ProgramLife.GetService<IAria2cClient>().LauncherAsync(new()
        {
            SesionFilePath = "D:\\save.session"
        });
        await ProgramLife.GetService<IAria2cClient>().ConnectAsync();
    }
}
