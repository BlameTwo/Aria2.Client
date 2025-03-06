using Aria2.Client.Common;
using Aria2.Client.ViewModels.DialogViewModels;
using Microsoft.UI.Xaml.Controls;
namespace Aria2.Client.Views.Dialogs;

public sealed partial class AddUriDialog : ContentDialog, IDialogBase<string>
{
    public AddUriDialog()
    {
        InitializeComponent();
        ViewModel = ProgramLife.GetService<AddUriViewModel>();
    }

    public AddUriViewModel ViewModel { get; }

    public void SetData(string data)
    {
        ViewModel.TextUri = data;
    }
}
