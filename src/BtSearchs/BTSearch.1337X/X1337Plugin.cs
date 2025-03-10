﻿using BTSearch._1337X.Providers;
using CommunityToolkit.Mvvm.ComponentModel;
using HtmlAgilityPack;
using IBtSearch;
using IBtSearch.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BTSearch._1337X;

public partial class X1337Plugin : ObservableObject, IBTSearchPlugin
{
    /// <summary>
    /// 是否支持编辑
    /// </summary>
    public bool IsEditerConfig { get; private set; }

    public string Guid => "BAA7032E-0DB0-2454-124F-E829283C1FA0";

    public string Name => "1337X";

    public string Orgin => "https://www.1337xx.to/";

    public string Icon => "https://www.1337xx.to/favicon.ico";

    HttpClientProvider HttpClientProvider { get; set; }

    public string Version => "0.1 Bate";

    public X1337Plugin()
    {
        HttpClientProvider = new HttpClientProvider(true);
    }

    public async IAsyncEnumerable<BTSearchResult> SearchAsync(
        string query,
        [EnumeratorCancellation] CancellationToken token = default
    )
    {
        HttpClient client = new();
        var page = 1;
        var nowPage = 1;
        var searchUrl = $"https://www.1337xx.to/search/{query}/{nowPage}/";
        HtmlDocument doc = new();
        var content = await (await client.GetAsync(searchUrl, token)).Content.ReadAsStringAsync();
        doc.LoadHtml(content);
        #region  页数
        var pages = doc.DocumentNode.SelectNodes("//div[@class='pagination']/ul//li");
        string pageLast = pages.Last().InnerText;
        page += Convert.ToInt32(pageLast);
        do
        {
            HtmlNodeCollection nodes = null;
            if (nowPage > 1)
            {
                //说明不是请求的第一页
                var pageHttp = await HttpClientProvider.Client.GetAsync(
                    $"https://www.1337xx.to/search/{query}/{nowPage}/",
                    token
                );
                var pageContent = await pageHttp.Content.ReadAsStringAsync();
                doc.LoadHtml(pageContent);
                var table = doc.DocumentNode.SelectSingleNode("//table//tbody");
                var trs = table.SelectNodes(".//tr");
                nodes = trs;
            }
            else
            {
                var table = doc.DocumentNode.SelectSingleNode("//table//tbody");
                var trs = table.SelectNodes(".//tr");
                nodes = trs;
            }
            nowPage++;
            foreach (var tr in nodes)
            {
                BTSearchResult bTSearchResult = new BTSearchResult();
                var titletd = tr.SelectSingleNode(".//td[1]//a[2]");
                var title = titletd.InnerText;
                bTSearchResult.Name = title;
                var url = titletd.GetAttributeValue<string>("href", "");
                bTSearchResult.WebUrl = url;
                var tdContent = await (
                    await client.GetAsync($"https://www.1337xx.to{url}", token)
                ).Content.ReadAsStringAsync();
                HtmlDocument htl = new();
                if (tdContent == "error code: 521")
                    continue;
                htl.LoadHtml(tdContent);
                var btSession = htl.DocumentNode.SelectSingleNode(
                    "//main[@class='container']/div/div//div[2]//div"
                );
                var bt = btSession.SelectSingleNode(".//ul[1]//li[1]//a");
                var leftbt = btSession.SelectSingleNode(".//ul[2]");
                var rightbt = btSession.SelectSingleNode(".//ul[3]");
                bTSearchResult.BTUrl = bt.GetAttributeValue<string>("href", null);
                bTSearchResult.Size = leftbt.SelectSingleNode(".//li[4]//span").InnerText;
                bTSearchResult.CreateTime = FormatDateTime(
                    rightbt.SelectSingleNode(".//li[3]//span").InnerText
                );
                bTSearchResult.MaxPageCount = page;
                bTSearchResult.BTUrl = bt.GetAttributeValue<string>("href", "");
                yield return bTSearchResult;
            }
        } while (!(nowPage > page));
        #region 列表数据

        #endregion
        #endregion
    }

    DateTime FormatDateTime(string dateString)
    {
        string format = "MMM dd yy";
        // 使用正则表达式移除日期中的序数后缀
        dateString = Regex.Replace(dateString, @"\b(\d+)(st|nd|rd|th)\b", "$1");
        // 移除点号后的空格
        dateString = dateString.Replace(". ", " ").Replace("'", "");
        DateTime dateTime = DateTime.ParseExact(dateString, format, new CultureInfo("en-US"));
        Console.WriteLine(dateTime); // 输出: 2018-09-01 00:00:00
        return dateTime;
    }

    public string JsonPath { get; private set; }

    public PluginConfig Config { get; set; }

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

}
