using Aria2.Client.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace Aria2.Client.Views;

public sealed partial class PluginPage : Page
{
    public PluginPage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<PluginViewModel>();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel.Unregister();
        this.ViewModel = null;
        GC.Collect();
        base.OnNavigatedFrom(e);
    }

    public PluginViewModel ViewModel { get; private set; }

}
