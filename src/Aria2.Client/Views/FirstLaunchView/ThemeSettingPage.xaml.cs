using Aria2.Client.ViewModels.FirstLaunchViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.FirstLaunchView;

public sealed partial class ThemeSettingPage : Page
{
    public ThemeSettingPage()
    {
        InitializeComponent();
        ViewModel = ProgramLife.GetService<ThemeSettingViewModel>();
    }

    public ThemeSettingViewModel ViewModel { get; }
}