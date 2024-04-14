
using Aria2.Client.Models.Anime;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Principal;
using System.ServiceModel.Syndication;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace Aria2.Client.Services.Contracts;

public class OnekumaService:IOnekumaService
{
    private HttpClient _client;

    public OnekumaService()
    {
        this._client = new HttpClient();
    }
    public void AddUrl(string url)
    {
        XmlReader reader = XmlReader.Create(url);
        SyndicationFeed feed= SyndicationFeed.Load(reader);
    }

    public async Task<AnimeTorrentModel> GetAnimeHomeAsync(int page, int pagesize)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new System.Uri($"https://garden.onekuma.cn/api/resources?page={page}"),
            Method = HttpMethod.Get,
        };
        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        var reponse = await _client.SendAsync(request);
        var json = await reponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<AnimeTorrentModel>(json);
        if (result == null)
            return null;
        return result;
    }


    public async Task<AnimeTorrentModel> SearchKeyworkd(List<string> keyword, string Type = null,int page = 1,int pageSize = 20)
    {
        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
        };
        var content = JsonContent.Create(new { include = keyword });
        var str = await content.ReadAsStringAsync();
        string url = "https://garden.onekuma.cn/api/resources";
        if (Type != null)
            url += $"?type={Type}&page=1&pageSize=20";
        request.RequestUri = new System.Uri(url);
        request.Content = content;
        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7"); 
        var reponse = await _client.SendAsync(request);
        var json = await reponse.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<AnimeTorrentModel>(json);
        if (result == null)
            return null;
        return result;
    }
}