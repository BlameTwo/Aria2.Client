using Aria2.Client.Common;
using Aria2.Client.Services.Contracts;
using BtSearch.Loader.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using IBtSearch;
using System;
using System.Collections.ObjectModel;

namespace Aria2.Client.ViewModels;

public partial class PluginViewModel : PageViewModelBase
{
    public PluginViewModel(IPluginManager pluginManager) : base("插件")
    {
        PluginManager = pluginManager;
        this.Plugins = new(PluginManager.Plugins);
    }

    [ObservableProperty]
    ObservableCollection<Tuple<PluginContextLoader, IBTSearchPlugin>> _Plugins;

    public IPluginManager PluginManager { get; }
}
