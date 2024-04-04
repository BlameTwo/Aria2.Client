using Aria2.Client.Services.Contracts;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Aria2.Client.Services;

public class PickersService : IPickersService
{
    public PickersService(IApplicationSetup<App> applicationSetup)
    {
        ApplicationSetup = applicationSetup;
    }

    public IApplicationSetup<App> ApplicationSetup { get; }

    public FileOpenPicker GetFileOpenPicker()
    {
        throw new System.NotImplementedException();
    }

    public FileSavePicker GetFileSavePicker()
    {
        throw new System.NotImplementedException();
    }

    public async Task<StorageFolder> GetFolderPicker()
    {
        FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(ApplicationSetup.Application.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        return await openPicker.PickSingleFolderAsync();
    }
}
