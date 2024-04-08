using Aria2.Client.Models.Bases;
using CommunityToolkit.Mvvm.Input;
using IBtSearch.Bases;
using System.Threading.Tasks;

namespace Aria2.Client.Models;

public partial class BTPluginItemData: ItemDownloadBase<IAria2Plugin>
{
    [RelayCommand]
    async Task SetEnable()
    {
       await this.Data.SetEnabledAsync();
    }
}
