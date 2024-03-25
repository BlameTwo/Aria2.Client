using Aria2.Client.ViewModels.DownloadViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.DownloadPages;

public sealed partial class StopPage : Page
{
    public StopPage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<StopViewModel>();
    }

    public StopViewModel ViewModel { get; }

    private void Page_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.OnUnLoad();
    }
}
