using BtSearch.Fitgril;
using BTSearch.Mikanime;
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

    [TestMethod]
    public async Task MGJHText()
    {
        MikanimePlugin plugin = new();
        await foreach (var item in plugin.SearchAsync("鬼父"))
        {
            Debug.WriteLine(item.Name + "        " + item.BTUrl);
        }
    }
}
