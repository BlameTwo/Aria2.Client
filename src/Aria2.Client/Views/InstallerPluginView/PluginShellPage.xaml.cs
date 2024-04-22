using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels.InstallerPluginViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.InstallerPluginView;

public sealed partial class PluginShellPage : Page
{
    public PluginShellPage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<PluginShellViewModel>();
        Loaded += PluginShellPage_Loaded;
    }

    private void PluginShellPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        this.titlebar.Window = ProgramLife.GetService<IApplicationSetup<App>>().Application.MainWindow;

        this.ViewModel.DialogManager.RegisterRoot(this.XamlRoot);
        this.ViewModel.TipShow.Owner = this.grid;
    }

    public PluginShellViewModel ViewModel { get; }
}