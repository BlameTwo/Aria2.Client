using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using IBtSearch;
using IBtSearch.Models;
using IBtSearch.Providers;

namespace BTSearch.Mikanime;

public class MikanimePlugin : IBTSearchPlugin
{
    HttpClientProvider HttpClientProvider { get; }
    public string Orgin => "https://mikanime.tv";

    public string Guid => "7E802B2B-CCB4-4BB3-B810-2E49C5E3E5FB";

    public string Name => "蜜柑计划";

    public string Icon => "https://mikanime.tv/images/favicon.ico?v=2";

    public string Version => "1.0";

    public PluginConfig Config { get; set; }
    public string JsonPath { get; private set; }
    public bool IsEditerConfig { get; private set; }

    public async Task LoadConfig(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            IsEditerConfig = false;
            return;
        }
        JsonPath = folderPath + $"\\{Name}_Config.json";
        if (File.Exists(JsonPath))
        {
            Config =
                JsonSerializer.Deserialize<PluginConfig>(await File.ReadAllTextAsync(JsonPath))
                ?? new();
            return;
        }
        PluginConfig pluginConfig = new PluginConfig();
        pluginConfig.IsEnabled = true;
        pluginConfig.LastActive = DateTime.Now;
        using (var writer = File.CreateText(JsonPath))
        {
            await writer.WriteLineAsync(JsonSerializer.Serialize(pluginConfig) ?? "");
        }
        Config = pluginConfig;
    }

    public MikanimePlugin()
    {
        HttpClientProvider = new(true);
    }

    public async IAsyncEnumerable<BTSearchResult> SearchAsync(
        string query,
        [EnumeratorCancellation] CancellationToken token = default
    )
    {
        List<BTSearchResult> list = new List<BTSearchResult>();
        var http = await HttpClientProvider.Client.GetAsync(
          $"https://mikanime.tv/Home/Search?searchstr={query}",
          token
        );
        var content = await http.Content.ReadAsStringAsync(token);
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(content);
        var lists = doc.DocumentNode.SelectNodes("//table//tbody//tr");
        if (lists == null)
            yield break;
        foreach (var item in lists)
        {
            //https://mikanime.tv/
            BTSearchResult itemResult = new();
            var linkUrl = item.SelectSingleNode("./td//a[1]");
            var btlink = item.SelectSingleNode("./td//a[2]");
            itemResult.Name = System.Net.WebUtility.HtmlDecode(linkUrl.InnerText);
            itemResult.WebUrl = Orgin + linkUrl.GetAttributeValue<string>("href", "");
            itemResult.BTUrl = btlink.GetAttributeValue<string>("data-clipboard-text", "");
            itemResult.Size = item.SelectSingleNode(".//td[2]").InnerText;
            itemResult.CreateTime = DateTime.Parse(item.SelectSingleNode(".//td[3]").InnerText);
            itemResult.BTResultType = IBtSearch.Models.Enums.BtResultType.Magent;
            var sessionPat = await HttpClientProvider.Client.GetAsync(itemResult.WebUrl, token);
            var sessionDoc = new HtmlDocument();
            sessionDoc.LoadHtml(await sessionPat.Content.ReadAsStringAsync(token));
            var session = sessionDoc.DocumentNode.SelectSingleNode("//div[@id='sk-container']/div[1]/div");
            string pattern = @"url\((.*?)\)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(session.GetAttributeValue<string>("style",""));
            if(match.Success)
            {
                string urlWithQuotes = match.Groups[1].Value;
                string url = Orgin + urlWithQuotes.Trim(new char[] { '\'', '\"' });
                itemResult.Cover = url;
            }
            itemResult.MaxPageCount = -1;
            itemResult.NowPage = -1;
            yield return itemResult;
        }
    }



    public async Task SetEnabledAsync()
    {
        await SaveAsync();
    }

    async Task SaveAsync()
    {
        await File.WriteAllTextAsync(JsonPath, JsonSerializer.Serialize(Config));
    }

    public async Task SetUninstall()
    {
        await SaveAsync();
    }
}
