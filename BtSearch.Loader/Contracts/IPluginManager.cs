using BtSearch.Loader.Models;
using IBtSearch;
using System;
using System.Collections.Generic;

namespace Aria2.Client.Services.Contracts;

public interface IPluginManager
{
    public IAsyncEnumerable<IBTSearchPlugin> GetSearchPlugins(CancellationToken token = default);
}
