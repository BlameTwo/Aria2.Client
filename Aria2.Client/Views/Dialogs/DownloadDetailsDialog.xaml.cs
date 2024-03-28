using Aria2.Client.Common;
using Aria2.Client.ViewModels.DialogViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.Dialogs;

public sealed partial class DownloadDetailsDialog : ContentDialog, IDialogBase<string>
{
    public DownloadDetailsDialog()
    {
        this.InitializeComponent();
        this.ViewModel = ProgramLife.GetService<DownloadDetailsViewModel>();
    }


    public DownloadDetailsViewModel ViewModel { get; private set; }

    public async void SetData(string data)
    {
        await this.ViewModel.RefreshTask(data);
    }
}