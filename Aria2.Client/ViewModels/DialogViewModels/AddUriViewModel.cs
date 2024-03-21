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

namespace Aria2.Client.ViewModels.DialogViewModels;

public sealed partial class AddUriViewModel:ObservableRecipient
{
    public AddUriViewModel(IDialogManager dialogManager,IApplicationSetup<App> applicationSetup)
    {
        DialogManager = dialogManager;
        ApplicationSetup = applicationSetup;
    }

    public IDialogManager DialogManager { get; }
    public IApplicationSetup<App> ApplicationSetup { get; }

    [NotifyCanExecuteChangedFor(nameof(ActiveCommand))]
    [ObservableProperty]
    private bool _ActiveEnable;

    [ObservableProperty]
    string _TextUri;

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
    void Active()
    {

    }


    [RelayCommand]
    async Task SelectSaveFolder()
    {

        // Create a folder picker
        FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();

        // See the sample code below for how to make the window accessible from the App class.

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(ApplicationSetup.Application.MainWindow);

        // Initialize the folder picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options for your folder picker
        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");

        // Open the picker for the user to pick a folder
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
