using Aria2.Client.ViewModels.NotifyViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.NotifyViews;

public sealed partial class NotifyMainPage : Page
{
    public NotifyMainPage()
    {
        InitializeComponent();
        ViewModel  = ProgramLife.GetService<NotifyMainViewModel>();
    }

    public NotifyMainViewModel ViewModel { get; }
}