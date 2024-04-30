using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Aria2.Net.Services.Contracts;
using System.Collections.Generic;

namespace Aria2.Client.ViewModels.DialogViewModels;

public sealed partial class AddUriViewModel : ObservableRecipient
{
    public AddUriViewModel(
        IDialogManager dialogManager,
        IApplicationSetup<App> applicationSetup,
        IAria2cClient aria2CClient,
        IAppMessageService appMessageService
    )
    {
        DialogManager = dialogManager;
        ApplicationSetup = applicationSetup;
        Aria2CClient = aria2CClient;
        AppMessageService = appMessageService;
    }

    public IDialogManager DialogManager { get; }
    public IApplicationSetup<App> ApplicationSetup { get; }
    public IAria2cClient Aria2CClient { get; }
    public IAppMessageService AppMessageService { get; }

    [NotifyCanExecuteChangedFor(nameof(ActiveCommand))]
    [ObservableProperty]
    private bool _ActiveEnable;

    [ObservableProperty]
    string _TextUri = "";

    [ObservableProperty]
    string _HttpHeader = "";

    [ObservableProperty]
    string _UserAgent = "";

    [ObservableProperty]
    string _Refrere = "";

    [ObservableProperty]
    string _SavePath = UserDataPaths.GetDefault().Downloads;

    [ObservableProperty]
    SelectorBarItem _SelectTabIndex;

    [ObservableProperty]
    bool _IsUriShow;

    partial void OnSelectTabIndexChanged(SelectorBarItem value)
    {
        Debug.WriteLine(value.Text);
        if (value.Text == "URL")
            IsUriShow = true;
        else
            IsUriShow = false;
    }

    partial void OnTextUriChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            ActiveEnable = true;
            return;
        }
        ActiveEnable = false;
    }

    [RelayCommand(CanExecute = nameof(ActiveEnable))]
    async Task Active()
    {
        try
        {
            var list = TextUri.Split("\r\n");
            if (list == null || list.Length == 0)
                return;
            var option = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(this.UserAgent))
                option.Add("user-agent", UserAgent);
            if (!string.IsNullOrWhiteSpace(this.Refrere))
                option.Add("referer", UserAgent);
            if (!string.IsNullOrWhiteSpace(this.HttpHeader))
                option.Add("header", UserAgent);
            var result = await Aria2CClient.AddUriAsync(
                list,
                new Dictionary<string, object>()
                {
                    { "dir", this.SavePath },
                    { "follow-torrent", true }
                },
                1
            );
            AppMessageService.SendTimeSpanMessage(
                TimeSpan.FromSeconds(3),
                $"开始下载，任务ID{result.Result}",
                "Aria2",
                Models.Enums.MessageLevel.Default
            );

            if (result.Result != null)
                DialogManager.CloseDialog();
        }
        catch (Exception) { }
        finally
        {
            DialogManager.CloseDialog();
        }
    }

    [RelayCommand]
    async Task SelectSaveFolder()
    {
        FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(
            ApplicationSetup.Application.MainWindow
        );
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");
        StorageFolder folder = await openPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            this.SavePath = folder.Path;
        }
    }

    [RelayCommand]
    void Close()
    {
        DialogManager.CloseDialog();
    }
}
