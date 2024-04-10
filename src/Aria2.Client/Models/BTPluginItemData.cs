using Aria2.Client.Models.Bases;
using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IBtSearch.Bases;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace Aria2.Client.Models;

public partial class BTPluginItemData: ItemDownloadBase<IAria2Plugin>
{
    public BTPluginItemData(IAppMessageService appMessageService)
    {
        AppMessageService = appMessageService;
    }

    [RelayCommand]
    async Task SetEnable()
    {
        await this.Data.SetEnabledAsync();
        RefreshData();
    }

    [RelayCommand]
    async Task SetUninstall()
    {
        await this.Data.SetUninstall();
        RefreshData();
    }

    void RefreshData()
    {
        if (this.Data.Config.IsUninstall)
        {
            this.message = AppMessageService.SendMessage($"{Data.Name}卸载标识已标记，重启应用生效","插件消息", Enums.MessageLevel.Warn,false);
            this.IsUninstall = Visibility.Collapsed;
        }
        else
        {
            if(message != null)
                AppMessageService.ClearMessage(this.message);
        }
    }


    [ObservableProperty]
    Visibility _IsUninstall = Visibility.Visible;
    private string message = "";

    public IAppMessageService AppMessageService { get; }
}
