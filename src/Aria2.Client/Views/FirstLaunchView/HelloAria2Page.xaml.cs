using Aria2.Client.ViewModels.FirstLaunchViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.FirstLaunchView;

public sealed partial class HelloAria2Page : Page
{
    public HelloAria2Page(HelloAria2ViewModel vm)
    {
        this.InitializeComponent();
        this.ViewModel = vm;
    }

    public HelloAria2ViewModel ViewModel { get; }
}
