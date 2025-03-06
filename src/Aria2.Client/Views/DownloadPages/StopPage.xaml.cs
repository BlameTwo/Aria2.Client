using Aria2.Client.ViewModels.DownloadViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace Aria2.Client.Views.DownloadPages;

public sealed partial class StopPage : Page
{
    public StopPage()
    {
        InitializeComponent();
        ViewModel = ProgramLife.GetService<StopViewModel>();
    }

    public StopViewModel ViewModel { get; private set; }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        ViewModel.Unregister();
        ViewModel = null;
        GC.Collect();
        base.OnNavigatingFrom(e);
    }

    private void Page_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //ViewModel.OnUnLoad();
    }
}
