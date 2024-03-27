using Aria2.Client.Common;
using Aria2.Client.Services.Contracts;
using Aria2.Client.Views.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Threading.Tasks;

namespace Aria2.Client.Services;

public class DialogManager : IDialogManager
{
    public XamlRoot Root { get; private set; }

    public ContentDialog _dialog = null;

    public void CloseDialog()
    {
        if (_dialog != null)
        {
            _dialog.Hide();
        }
    }

    public void RegisterRoot(XamlRoot root)
    {
        this.Root = root;
    }

    public async Task ShowAddTorrentAsync()
        => await ShowDialogAsync<AddTorrentDialog>();

    public async Task ShowAddUriAsync()
        => await ShowDialogAsync<AddUriDialog>();

    private async Task ShowDialogAsync<T>()
        where T:ContentDialog
    {
        var dialog = ProgramLife.GetService<T>();
        if (dialog == null) return;
        dialog.XamlRoot = Root;
        this._dialog = dialog;
        await dialog.ShowAsync();
    }

    private async Task ShowDialogAsync<T, Value>(Value type) where T : ContentDialog, IDialogBase<Value>
    {
        var dialog = ProgramLife.GetService<T>();
        if (dialog == null) return;
        dialog.XamlRoot = Root;
        dialog.SetData(type);
        await dialog.ShowAsync();
    }

    public async Task ShowDownloadDetails(string gid)
        =>await ShowDialogAsync<DownloadDetailsDialog,string>(gid);

}
