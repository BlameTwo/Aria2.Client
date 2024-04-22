using Aria2.Client.ViewModels.NotifyViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.NotifyViews;

public sealed partial class NotyfiMainPage : Page
{
    public NotyfiMainPage()
    {
        this.InitializeComponent();
        this.ViewModel  = ProgramLife.GetService<NotifyMainViewModel>();
    }

    public NotifyMainViewModel ViewModel { get; }
}