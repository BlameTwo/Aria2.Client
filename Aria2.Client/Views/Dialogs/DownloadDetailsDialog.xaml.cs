using Aria2.Client.Common;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.Dialogs;

public sealed partial class DownloadDetailsDialog : ContentDialog, IDialogBase<string>
{
    public DownloadDetailsDialog()
    {
        this.InitializeComponent();
    }

    public void SetData(string data)
    {

    }
}