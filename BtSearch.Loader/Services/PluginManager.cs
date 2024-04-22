using Aria2.Client.Services.Contracts;
using BtSearch.Loader.Models;
using IBtSearch;
using IBtSearch.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BtSearch.Loader.Services;

public class PluginManager : IPluginManager
{
    public PluginManager()
    {
        SearchPlugins = new List<IBTSearchPlugin>();
    }

    public async IAsyncEnumerable<IBTSearchPlugin> GetSearchPlugins([EnumeratorCancellation]CancellationToken token = default)
    {
        bool flage = true;
        if (this.SearchPlugins.Count > 0)
        {
            foreach (var item in SearchPlugins)
            {
                yield return item;
                GC.Collect();
                flage = false;
            }
        }
        if (flage)
        {
            var folder = new DirectoryInfo(Aria2Config.PluginPath);
            var folders = folder.GetDirectories();
            for (int i = 0; i < folders.Length; i++)
            {
                if (!Directory.Exists(folders[i].FullName))
                    continue;
                foreach (var path in folders[i].GetFiles("*.dll"))
                {
                    var result = await LoadSingleSearchAsync(path.FullName);
                    if (result == null)
                        continue;
                    this.SearchPlugins.Add(result);
                    GC.Collect();

                    yield return result;
                }
            }
        }
    }

    async Task<IBTSearchPlugin> LoadSingleSearchAsync(string path, CancellationToken token = default)
    {
        PluginConfig config = null;
        var folderName = System.IO.Path.GetDirectoryName(path);
        if (!Directory.Exists(folderName))
            return null;
        foreach (var item in new DirectoryInfo(folderName).GetFiles("*.json"))
        {
            try
            {
                var jsonstr = JsonSerializer.Deserialize<PluginConfig>(await File.ReadAllTextAsync(item.FullName));
                if (jsonstr.LastActive == DateTime.MinValue && jsonstr.IsEnabled == false)
                {
                    config = new();
                    continue;
                }
                config = jsonstr;
                break;
            }
            catch (Exception)
            {
                return null;
            }
        }
        if (config == null)
            config = new();
        if (config.IsUninstall)
        {
            Directory.Delete(System.IO.Path.GetDirectoryName(path)!, true);
            return null;
        }
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            PluginContextLoader loader = new(path);
            var ass = loader.LoadFromStream(fs);
            foreach (var type in ass.GetTypes())
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                var interfaces = type.GetInterfaces();
                foreach (var item in interfaces)
                {
                    if (typeof(IBTSearchPlugin).IsAssignableFrom(item))
                    {
                        IBTSearchPlugin result = Activator.CreateInstance(type) as IBTSearchPlugin;
                        if (result == null)
                            break;
                        await result.LoadConfig(Path.GetDirectoryName(path));
                        return result;
                    }
                }
            }
        }
        return default;
    }

    public IList<IBTSearchPlugin> SearchPlugins { get; private set; }
}
