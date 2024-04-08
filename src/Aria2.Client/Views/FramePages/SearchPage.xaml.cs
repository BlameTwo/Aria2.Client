using Aria2.Client.ViewModels.FrameViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
namespace Aria2.Client.Views.FramePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
            this.ViewModel = ProgramLife.GetService<SearchViewModel>();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            this.ViewModel.Unregister();
            this.ViewModel = null;
            GC.Collect();
            base.OnNavigatingFrom(e);
        }

        public SearchViewModel ViewModel { get; private set; }
    }
}
