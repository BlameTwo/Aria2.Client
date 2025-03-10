using Aria2.Client.ViewModels.FirstLaunchViewModel;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.FirstLaunchView;

public sealed partial class FileSettingsPage : Page
{
    public FileSettingsPage()
    {
        InitializeComponent();
        ViewModel = ProgramLife.GetService<FileSettingsViewModel>();
    }

    public FileSettingsViewModel ViewModel { get; }
}
