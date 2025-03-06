using Aria2.Client.ViewModels.DownloadViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace Aria2.Client.Views.DownloadPages
{
    public sealed partial class ActivePage : Page
    {
        public ActivePage()
        {
            InitializeComponent();
            ViewModel = ProgramLife.GetService<ActiveViewModel>();
        }

        public ActiveViewModel ViewModel { get; private set; }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.Unregister();
            ViewModel = null;
            GC.Collect();
            base.OnNavigatingFrom(e);
        }

    }
}
