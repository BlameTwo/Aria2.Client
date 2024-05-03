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

    public async Task ShowAddUriAsync(string url)
        => await this.ShowDialogAsync<AddUriDialog,string>(url);

    private async Task ShowDialogAsync<T>()
        where T:ContentDialog
    {
        var dialog = ProgramLife.GetService<T>();
        if (dialog == null) return;
        dialog.XamlRoot = Root;
        this._dialog = dialog;
        await dialog.ShowAsync();
    }

    private async Task<ContentDialogResult> ShowResultDialogAsync<T>()
        where T:ContentDialog
    {
        var dialog = ProgramLife.GetService<T>();
        if (dialog == null) return ContentDialogResult.None;
        dialog.XamlRoot = Root;
        this._dialog = dialog;
        return await _dialog.ShowAsync();
    }

    private async Task ShowDialogAsync<T, Value>(Value type) where T : ContentDialog, IDialogBase<Value>
    {
        var dialog = ProgramLife.GetService<T>();
        if (dialog == null) return;
        dialog.XamlRoot = Root;
        this._dialog = dialog;
        dialog.SetData(type);
        await dialog.ShowAsync();
    }

    public async Task<ContentDialogResult> ExitApp()
    {
        return await this.ShowResultDialogAsync<ExitDialog>();
    }
}
