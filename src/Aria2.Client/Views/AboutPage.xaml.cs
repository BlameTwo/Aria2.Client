using Aria2.Client.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views;

public sealed partial class AboutPage : Page
{
    public AboutPage()
    {
        InitializeComponent();
        ViewModel = ProgramLife.GetService<AboutViewModel>();
    }

    public AboutViewModel ViewModel { get; }
}
