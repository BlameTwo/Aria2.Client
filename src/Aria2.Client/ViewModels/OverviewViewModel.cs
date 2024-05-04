using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Aria2.Client.Services.Contracts;
using Aria2.Net;
using Aria2.Net.Common;
using Aria2.Net.Models.Enums;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Microsoft.UI.Xaml;

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
    Aria2LauncherConfig _Config;

    [RelayCommand]
    async Task Loaded()
    {
        var options = await Aria2CClient.GetGlobalOption();
        this.Config = await LocalSettingsService.ReadObjectConfig<Aria2LauncherConfig>(
            "LauncherConfig",
            Ctr.Token
        );
        this.LogPath = Config.LogFilePath;
        this.SessionPath = Config.SesionFilePath;
        Match match = Regex.Match(Config.MaxDownloadSpeed, @"(\d+)([A-Z]+)");
        this.InputDownload = double.Parse(match.Groups[1].Value);
        this.SelectDownload = (match.Groups[2].Value);
        Match upload = Regex.Match(Config.MaxUploadSpeed, @"(\d+)([A-Z]+)");
        this.InputUpload = double.Parse(upload.Groups[1].Value);
        this.SelectUpload = (upload.Groups[2].Value);
        this.MaxResult = Config.MaxSaveResultCount;
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

    [ObservableProperty]
    ObservableCollection<ISeries> _Series;

    public IAria2cClient Aria2CClient { get; }
    public ITipShow TipShow { get; }
    public IPickersService PickersService { get; }
    public ILocalSettingsService LocalSettingsService { get; }
    public IAppMessageService AppMessageService { get; }
}
