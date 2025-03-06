using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

public partial class PluginViewModel : PageViewModelBase
{
    public PluginViewModel(IPluginManager pluginManager,IDataFactory dataFactory) : base("插件")
    {
        PluginManager = pluginManager;
        DataFactory = dataFactory;
    }

    [ObservableProperty]
    ObservableCollection<BTPluginItemData> _SearchPlugins=new();

    public override void Unregister()
    {
        SearchPlugins.Clear();
    }


    [RelayCommand]
    async Task Loaded()
    {
        await foreach (var item in PluginManager.GetSearchPlugins())
        {
            if (item == null)
                continue;
            SearchPlugins.Add(DataFactory.CreatePluginItem(item));
        }
    }

    public IPluginManager PluginManager { get; }
    public IDataFactory DataFactory { get; }
}
