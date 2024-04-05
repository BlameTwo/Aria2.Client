using Aria2.Client.ViewModels.DialogViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.Dialogs;

public sealed partial class AddTorrentDialog : ContentDialog
{
    public AddTorrentDialog()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<AddTorrentViewModel>();
    }

    public AddTorrentViewModel ViewModel { get; }
}
