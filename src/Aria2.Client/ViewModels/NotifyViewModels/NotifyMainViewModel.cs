using Aria2.Client.Common;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Models.ClientModel;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WinUIEx;

namespace Aria2.Client.ViewModels.NotifyViewModels;

public sealed partial class NotifyMainViewModel:ObservableRecipient
{
    DispatcherTimer _timer = null;
    public NotifyMainViewModel(IAria2cClient aria2CClient,IApplicationSetup<App> applicationSetup)
    {
        Aria2CClient = aria2CClient;
        ApplicationSetup = applicationSetup;
        _timer = new()
        {
            Interval = TimeSpan.FromSeconds(1),
            
        };
        _timer.Tick += _timer_Tick;
        _timer.Start();
    }

    private async void _timer_Tick(object sender, object e)
    {
        var tell = await Aria2CClient.GetAllTellStatus();
        this.Space = tell.Result;
    }

    public IAria2cClient Aria2CClient { get; }
    public IApplicationSetup<App> ApplicationSetup { get; }

    [ObservableProperty]
    AllTellStatus _Space;

    partial void OnSpaceChanged(AllTellStatus value)
    {
        this.ApplicationSetup.NotyfiIcon.ToolTipText = $"下载速度：{Math.Round(ByteConversion.BytesToMegabytes(long.Parse(value.DownloadSpeed)),2)} mb/s\r\n" +
            $"上传速度：{Math.Round(ByteConversion.BytesToMegabytes(long.Parse(value.UploadSpeed)),2)} mb/s";
    }

    [RelayCommand]
    async Task Exit()
    {
        await Aria2CClient.DisconnectAsync();
        await Aria2CClient.ExitAria2();
        Process.GetCurrentProcess().Kill();
    }

    [RelayCommand]
    void Show()
    {
        if(!ApplicationSetup.Application.MainWindow.AppWindow.IsVisible)
            ApplicationSetup.Application.MainWindow.Show();
    }
}
