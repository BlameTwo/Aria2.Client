using Aria2.Client.ViewModels.DownloadViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.DownloadPages
{
    public sealed partial class ActivePage : Page
    {
        public ActivePage()
        {
            this.InitializeComponent();
            this.ViewModel = ProgramLife.GetService<ActiveViewModel>();
        }

        public ActiveViewModel ViewModel { get; }
    }
}