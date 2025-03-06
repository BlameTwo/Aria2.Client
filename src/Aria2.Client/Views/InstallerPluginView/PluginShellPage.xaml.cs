using Aria2.Client.Services.Contracts;
using Aria2.Client.ViewModels.InstallerPluginViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.InstallerPluginView;

public sealed partial class PluginShellPage : Page
{
    public PluginShellPage()
    {
        InitializeComponent();
        ViewModel = ProgramLife.GetService<PluginShellViewModel>();
        Loaded += PluginShellPage_Loaded;
    }

    private void PluginShellPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        titlebar.Window = ProgramLife.GetService<IApplicationSetup<App>>().Application.MainWindow;

        ViewModel.DialogManager.RegisterRoot(XamlRoot);
        ViewModel.TipShow.Owner = grid;
    }

    public PluginShellViewModel ViewModel { get; }
}