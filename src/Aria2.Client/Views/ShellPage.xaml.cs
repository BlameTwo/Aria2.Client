using Aria2.Client.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views;

public sealed partial class ShellPage : Page
{
    public ShellPage()
    {
        InitializeComponent();
        ViewModel = ProgramLife.GetService<ShellViewModel>();
        ViewModel.NavigationService.RegisterView(frame);
        Loaded += ShellPage_Loaded;
    }

    private void ShellPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        TitleBar.Window = ViewModel.ApplicationSetup.Application.MainWindow;
        ViewModel.DialogManager.RegisterRoot(XamlRoot);
        ViewModel.ApplicationSetup.RegisterNotifyIcon(notifyIcon);
        ViewModel.TipShow.Owner = grid;
    }

    public ShellViewModel ViewModel { get; }

    private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        splitView.IsPaneOpen = !splitView.IsPaneOpen;
    }
}
