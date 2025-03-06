using Aria2.Client.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views;

public sealed partial class SettingPage : Page
{
    public SettingPage()
    {
        InitializeComponent();
        ViewModel = ProgramLife.GetService<SettingViewModel>();
       
    }

    public SettingViewModel ViewModel { get; }
}
