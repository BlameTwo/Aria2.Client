using Aria2.Client.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views;

public sealed partial class ShellPage : Page
{
    public ShellPage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<ShellViewModel>();
        this.ViewModel.NavigationService.RegisterView(frame);
        Loaded += ShellPage_Loaded;
    }

    private void ShellPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        this.TitleBar.Window = ViewModel.ApplicationSetup.Application.MainWindow;
        this.ViewModel.DialogManager.RegisterRoot(this.XamlRoot);
        this.ViewModel.TipShow.Owner = this.grid;
    }

    public ShellViewModel ViewModel { get; }
}
