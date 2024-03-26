using Aria2.Client.ViewModels.FrameViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.FramePages;

public sealed partial class AnimePage : Page
{
    public AnimePage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<AnimeViewModel>();
    }

    public AnimeViewModel ViewModel { get; }
}
