using Aria2.Client.ViewModels.FirstLaunchViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.FirstLaunchView;

public sealed partial class HelloAria2Page : Page
{
    public HelloAria2Page(HelloAria2ViewModel vm)
    {
        this.InitializeComponent();
        this.ViewModel = vm;
        this.ViewModel.NavigationService.RegisterView(this.frame);
        Loaded += HelloAria2Page_Loaded;
    }

    private void HelloAria2Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        this.titlebar.Window = ViewModel.ApplicationSetup.Application.MainWindow;
    }

    public HelloAria2ViewModel ViewModel { get; }

    private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        ViewModel.RefreshContent(args);
    }
}
