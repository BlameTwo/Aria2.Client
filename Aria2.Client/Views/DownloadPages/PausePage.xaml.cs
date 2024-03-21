using Aria2.Client.ViewModels.DownloadViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.DownloadPages;

public sealed partial class PausePage : Page
{
    public PausePage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<PauseViewModel>();
    }

    public PauseViewModel ViewModel { get; }
}
