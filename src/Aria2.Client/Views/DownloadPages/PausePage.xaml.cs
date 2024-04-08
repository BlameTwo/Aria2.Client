using Aria2.Client.ViewModels.DownloadViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace Aria2.Client.Views.DownloadPages;

public sealed partial class PausePage : Page
{
    public PausePage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<PauseViewModel>();
    }

    public PauseViewModel ViewModel { get; private set; }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        ViewModel.Unregister();
        this.ViewModel = null;
        GC.Collect();
        base.OnNavigatingFrom(e);
    }

    private void Page_Unloaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //ViewModel.OnUnLoad();
    }
}
