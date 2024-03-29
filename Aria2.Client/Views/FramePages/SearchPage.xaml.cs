using Aria2.Client.ViewModels.FrameViewModels;
using Microsoft.UI.Xaml.Controls;
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

        public SearchViewModel ViewModel { get; }
    }
}
