using Aria2.Client.ViewModels.DialogViewModels;
using Microsoft.UI.Xaml.Controls;
namespace Aria2.Client.Views.Dialogs;

public sealed partial class AddUriDialog : ContentDialog
{
    public AddUriDialog()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<AddUriViewModel>();
    }

    public AddUriViewModel ViewModel { get; }
}
