using IBtSearch;

namespace Aria2.Client.Services.Contracts;

public interface IPluginManager
{
    public IAsyncEnumerable<IBTSearchPlugin> GetSearchPlugins(CancellationToken token = default);
}
