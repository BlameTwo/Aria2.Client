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
        var result =  await plugin.SearchAsync("warm snow");
        foreach (var item in result)
        {
            Debug.WriteLine(item.Name + "        " + item.BTUrl);
        }
    }
}
