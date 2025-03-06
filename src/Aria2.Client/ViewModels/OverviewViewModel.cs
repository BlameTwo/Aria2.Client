using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Aria2.Client.Services.Contracts;
using Aria2.Net;
using Aria2.Net.Common;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;

namespace Aria2.Client.ViewModels;

public sealed partial class OverviewViewModel : ObservableRecipient
{
    CancellationTokenRegistration Ctr;


    public OverviewViewModel(
        IAria2cClient aria2CClient,
        ITipShow tipShow,
        IPickersService pickersService,
        ILocalSettingsService localSettingsService,
        IAppMessageService appMessageService
    )
    {
        Aria2CClient = aria2CClient;
        TipShow = tipShow;
        PickersService = pickersService;
        LocalSettingsService = localSettingsService;
        AppMessageService = appMessageService;
        Ctr = new();
        _downloadSpeed = new();
        _uploadSpeed = new();
        InitChart();
    }

    [ObservableProperty]
    public partial Aria2LauncherConfig Config { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<ISeries> Series { get; set; }

    [RelayCommand]
    async Task Loaded()
    {
        var options = await Aria2CClient.GetGlobalOption();
        Config = await LocalSettingsService.ReadObjectConfig<Aria2LauncherConfig>(
            "LauncherConfig",
            Ctr.Token
        );
        LogPath = Config.LogFilePath;
        SessionPath = Config.SessionFilePath;
        if(Config.MaxDownloadSpeed != "0")
        {
            Match match = Regex.Match(Config.MaxDownloadSpeed, @"(\d+)([A-Z]+)");
            InputDownload = double.Parse(match.Groups[1].Value);
            SelectDownload = (match.Groups[2].Value);
        }
        else
        {
            SelectDownload = "M";
        }
        if (Config.MaxUploadSpeed != "0")
        {
            Match upload = Regex.Match(Config.MaxUploadSpeed, @"(\d+)([A-Z]+)");
            InputUpload = double.Parse(upload.Groups[1].Value);
            SelectUpload = (upload.Groups[2].Value);
        }
        else
        {
            SelectUpload = "M";
        }
        MaxResult = Config.MaxSaveResultCount;
        var trackers = await ProgramLife
            .GetService<ILocalSettingsService>()
            .ReadObjectConfig<List<string>>("Trackers");
        if (trackers != null || trackers.Count > 0)
        {
            TrackerLines = string.Join("\r", trackers);
        }
    }

    void ShowResult(string message)
    {
        if (message == null)
            TipShow.ShowMessage("修改失败！", Microsoft.UI.Xaml.Controls.Symbol.Clear);
        if (message == GlobalUsings.RequestOK)
            TipShow.ShowMessage("修改配置成功", Microsoft.UI.Xaml.Controls.Symbol.Accept);
    }

    public IAria2cClient Aria2CClient { get; }

    public ITipShow TipShow { get; }

    public IPickersService PickersService { get; }

    public ILocalSettingsService LocalSettingsService { get; }

    public IAppMessageService AppMessageService { get; }
}
