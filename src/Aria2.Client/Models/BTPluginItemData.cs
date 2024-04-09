using Aria2.Client.Models.Bases;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IBtSearch.Bases;
using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace Aria2.Client.Models;

public partial class BTPluginItemData: ItemDownloadBase<IAria2Plugin>
{
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
            this.IsUninstall = Visibility.Collapsed;
    }


    [ObservableProperty]
    Visibility _IsUninstall = Visibility.Visible;
}
