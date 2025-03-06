using Microsoft.UI.Xaml.Controls;
using Aria2.Client.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Aria2.Client.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OverviewPage : Page
    {
        public OverviewPage()
        {
            InitializeComponent();
            ViewModel = ProgramLife.GetService<OverviewViewModel>();
        }



        public OverviewViewModel ViewModel { get; }
    }
}
