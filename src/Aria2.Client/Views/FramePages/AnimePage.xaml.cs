using Aria2.Client.ViewModels.FrameViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace Aria2.Client.Views.FramePages;

public sealed partial class AnimePage : Page
{
    public AnimePage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<AnimeViewModel>();
        
    }


    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        this.ViewModel.Dispose();
        this.ViewModel = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        base.OnNavigatedFrom(e);
    }

    public AnimeViewModel ViewModel { get; private set; }
}
