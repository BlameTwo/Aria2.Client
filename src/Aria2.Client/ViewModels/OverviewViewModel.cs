using Aria2.Client.Services.Contracts;
using Aria2.Net;
using Aria2.Net.Models.Enums;
using Aria2.Net.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

namespace Aria2.Client.ViewModels;

public sealed partial class OverviewViewModel : ObservableRecipient
{
    public OverviewViewModel(IAria2cClient aria2CClient,ITipShow tipShow,IPickersService pickersService)
    {
        Aria2CClient = aria2CClient;
        TipShow = tipShow;
        PickersService = pickersService;
    }

    void ShowResult(string message)
    {
        if (message == null)
            TipShow.ShowMessage("修改失败！", Microsoft.UI.Xaml.Controls.Symbol.Clear);
        if(message == GlobalUsings.RequestOK)
            TipShow.ShowMessage("修改配置成功", Microsoft.UI.Xaml.Controls.Symbol.Accept);
    }

    public IAria2cClient Aria2CClient { get; }
    public ITipShow TipShow { get; }
    public IPickersService PickersService { get; }
}
