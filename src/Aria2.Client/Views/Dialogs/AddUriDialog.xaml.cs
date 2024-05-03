using Aria2.Client.Common;
using Aria2.Client.ViewModels.DialogViewModels;
using Microsoft.UI.Xaml.Controls;
namespace Aria2.Client.Views.Dialogs;

public sealed partial class AddUriDialog : ContentDialog, IDialogBase<string>
{
    public AddUriDialog()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<AddUriViewModel>();
    }

    public AddUriViewModel ViewModel { get; }

    public void SetData(string data)
    {
        this.ViewModel.TextUri = data;
    }
}
