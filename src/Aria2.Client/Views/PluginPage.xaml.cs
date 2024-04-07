using Aria2.Client.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views;

public sealed partial class PluginPage : Page
{
    public PluginPage()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<PluginViewModel>();
    }

    public PluginViewModel ViewModel { get; }
}
