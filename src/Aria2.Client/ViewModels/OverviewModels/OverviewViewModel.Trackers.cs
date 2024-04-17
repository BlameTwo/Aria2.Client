using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls.TextToolbarSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

partial class OverviewViewModel
{
    [ObservableProperty]
    string _TrackerLines;

    [RelayCommand]
    async Task SaveTracker()
    {
        List<string> result = new();
        foreach (var item in TrackerLines.Split("\r"))
        {
            Regex UriRegex = new Regex(
                @"^(?<scheme>[a-zA-Z][a-zA-Z0-9+\-.]*):\/\/(?:(?<authority>[^\/:?#]*)@)?(?<host>[^\/?#:]*)(?::(?<port>\d+))?(?<path>[^\?#]*)?(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase
            );
            var match = UriRegex.Match(item);
            if (match.Success)
            {
                result.Add(item);
            }
        }
        await this.LocalSettingsService.SaveConfig(
            "Trackers",
            result,
            this.Ctr.Token
        );
        this.TrackerLines = string.Join("\r", result);
        TipShow.ShowMessage($"重启应用以启动新设置的Trackers列表", Microsoft.UI.Xaml.Controls.Symbol.Add);
        AppMessageService.SendTimeSpanMessage(TimeSpan.FromMinutes(2), "重启应用以设置最新的Trackers列表", "应用消息", Models.Enums.MessageLevel.Default);
    }
}
