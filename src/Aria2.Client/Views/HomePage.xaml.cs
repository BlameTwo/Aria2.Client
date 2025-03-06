using Aria2.Client.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views;

public sealed partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
        ViewModel = ProgramLife.GetService<HomeViewModel>();
        ViewModel.NavigationService.RegisterView(frame);
        ViewModel.NavigationViewService.Register(navigation);
    }

    public HomeViewModel ViewModel { get; }

    private void Page_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {

    }
}
