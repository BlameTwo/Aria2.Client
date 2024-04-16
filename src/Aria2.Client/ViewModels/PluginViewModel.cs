using Aria2.Client.Common;
using Aria2.Client.Common.ViewModelBase;
using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using BtSearch.Loader.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IBtSearch;
using System;
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
        this.SearchPlugins.Clear();
    }


    [RelayCommand]
    async Task Loaded()
    {
        await foreach (var item in PluginManager.GetSearchPlugins())
        {
            if (item == null)
                continue;
            this.SearchPlugins.Add(DataFactory.CreatePluginItem(item));
        }
    }

    public IPluginManager PluginManager { get; }
    public IDataFactory DataFactory { get; }
}
