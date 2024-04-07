using BtSearch.Fitgril;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Aria2.Test;

[TestClass]
public class PluginTest
{
    [TestMethod]
    public async Task FitgrilSearchAsync()
    {
        FitgrilPlugin plugin = new();
        await foreach (var item in plugin.SearchAsync("Warm Snow"))
        {
            Debug.WriteLine(item.Name + "        " + item.BTUrl);
        }
    }
}
