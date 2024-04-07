using Aria2.Client.Services.Contracts;
using BtSearch.Loader.Models;
using IBtSearch;
using System.Reflection;
using System.Windows.Input;

namespace BtSearch.Loader.Services;

public class PluginManager : IPluginManager
{
    public PluginManager()
    {
        _plugin = new List<Tuple<PluginContextLoader, IBTSearchPlugin>>();
        Load();
    }

    private void Load()
    {
        try
        {
            var folder = new DirectoryInfo(Aria2Config.PluginPath);
            var folders = folder.GetDirectories();
            for (int i = 0; i < folders.Length; i++)
            {
                foreach (var path in folders[i].GetFiles("*.dll"))
                {
                    PluginContextLoader loader = new(path.FullName);
                    var ass = loader.LoadFromAssemblyPath(path.FullName);
                    foreach (var type in ass.GetTypes())
                    {
                        var interfaces = type.GetInterfaces();
                        foreach (var item in interfaces)
                        {
                            if (typeof(IBTSearchPlugin).IsAssignableFrom(item))
                            {
                                IBTSearchPlugin result = Activator.CreateInstance(type) as IBTSearchPlugin;
                                if (result == null)
                                    break;
                                this._plugin.Add(new(loader, result));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private IList<Tuple<PluginContextLoader, IBTSearchPlugin>> _plugin;

    public IList<Tuple<PluginContextLoader, IBTSearchPlugin>> Plugins
    {
        get
        {
            return _plugin;
        }
        private set
        {
            _plugin = Plugins;
        }
    }
}
