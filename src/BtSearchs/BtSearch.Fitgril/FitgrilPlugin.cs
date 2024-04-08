using BtSearch.Fitgril.Providers;
using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using IBtSearch;
using IBtSearch.Bases;
using IBtSearch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BtSearch.Fitgril;

public partial class FitgrilPlugin : IBTSearchPlugin
{
    #region 声明
    public HttpClientProvider HttpClientProvider { get; private set; }
    #endregion
    public FitgrilPlugin()
    {
        HttpClientProvider = new(true);
    }

    public string Version => "0.1 Bate";

    public string Guid => "7FABBD92-0003-240B-6CCD-B39EBFCD2D49";
    public string Name => "Fitgril Repacks";
    public string Orgin => "https://fitgirl-repacks.site/";

    public string Icon => "https://fitgirl-repacks.site/wp-content/uploads/2016/08/cropped-icon-270x270.jpg";

    public PluginConfig Config { get; set; }

    public bool IsEditerConfig { get; private set; }

    public async Task SetEnabledAsync()
    {
        await File.WriteAllTextAsync(JsonPath, JsonSerializer.Serialize(Config));
    }

    public string JsonPath { get; private set; }

    public async IAsyncEnumerable<BTSearchResult> SearchAsync(
        string query,
        [EnumeratorCancellation] CancellationToken token = default
    )
    {
        List<BTSearchResult> list = new List<BTSearchResult>();
        var http = await HttpClientProvider.Client.GetAsync(
            $"https://fitgirl-repacks.site/?s={query}",
            token
        );
        var content = await http.Content.ReadAsStringAsync();
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(content);
        int page = 1;
        int nowpage = 1;
        var last = doc.DocumentNode.SelectSingleNode(
            "//span[@class='page-numbers dots']/following-sibling::a[1]"
        );
        if (last != null)
        {
            var pageResult = last.InnerText;
            page += Convert.ToInt32(pageResult);
        }
        do
        {
            HtmlNodeCollection nodes = null;
            if (nowpage > 1)
            {
                //说明不是请求的第一页
                var pageHttp = await HttpClientProvider.Client.GetAsync(
                    $"https://fitgirl-repacks.site/page/{nowpage}/?s={query}",
                    token
                );
                var pageContent = await pageHttp.Content.ReadAsStringAsync();
                doc.LoadHtml(pageContent);
                nodes = doc.DocumentNode.SelectNodes("//article");
            }
            else
            {
                nodes = doc.DocumentNode.SelectNodes("//article");
            }
            nowpage++;
            foreach (var node in nodes)
            {
                if (token != null)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();
                }
                #region 获得结果
                BTSearchResult result = new BTSearchResult();
                var title = node.SelectSingleNode(".//header//h1/a");
                result.Name = HttpUtility.HtmlDecode(title.InnerText);
                var des = node.SelectSingleNode(".//div/p");
                if (des == null)
                {
                    continue;
                }
                result.Description = HttpUtility.HtmlDecode(des.InnerText);
                var time = node.SelectSingleNode(".//div[2]/span/a/time");
                if (time != null)
                    result.CreateTime = DateTime.Parse(
                        time.GetAttributeValue<string>("datetime", null)
                    );
                var href = title.GetAttributeValue<string>("href", null);
                result.WebUrl = href;
                var session = await HttpClientProvider.Client.GetAsync(href, token);
                #region 获取图片
                var articledoc = new HtmlDocument();
                articledoc.LoadHtml(await session.Content.ReadAsStringAsync());
                var article = articledoc.DocumentNode.SelectSingleNode("//article");
                if (article == null)
                    continue;
                var image = article.SelectSingleNode("//div//p[1]/a/img");
                if (image == null)
                    continue;
                result.Cover = image.GetAttributeValue<string>("src", null);
                var size = article.SelectSingleNode("//div//p[1]/strong[5]");
                if (size == null)
                    continue;
                result.Size = size.InnerHtml;
                #endregion
                #region 获得下载链接
                var entrycontent = articledoc.DocumentNode.SelectSingleNode("//a[text()='magnet']");
                result.BTUrl = entrycontent.GetAttributeValue<string>("href", null);
                result.BTResultType = IBtSearch.Models.Enums.BtResultType.Magent;
                result.NowPage = nowpage;
                result.MaxPageCount = page;
                #endregion
                #endregion
                yield return result;
            }
        } while (!(nowpage > page));
    }

    public async Task LoadConfig(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            this.IsEditerConfig = false;
            return;
        }

        JsonPath = folderPath + $"\\{Name}_Config.json";
        if (File.Exists(JsonPath))
        {
            this.Config =
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
        this.Config = pluginConfig;
    }


}