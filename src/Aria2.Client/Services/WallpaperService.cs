using Aria2.Client.Models.Wallpaper;
using Aria2.Client.Services.Contracts;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Client.Services;

public sealed partial class WallpaperService : IWallpaperService
{
    HttpClient Client { get; }

    public WallpaperService()
    {
        Client = new();
    }

    public async Task<YurikotoModel> GetYurikoWallpaper(CancellationToken canceltoken = default)
    {
        HttpRequestMessage message = new(HttpMethod.Get, "https://v1.yurikoto.com/wallpaper?encode=json");
        var reponse = await Client.SendAsync(message, canceltoken);
        var jsonStr = await reponse.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<YurikotoModel>(jsonStr);
    }

    public async Task<BingImage> GetSingleBingWallpaper(CancellationToken token = default)
    {

        HttpRequestMessage message = new(HttpMethod.Get, "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=zh-CN");
        var reponse = await Client.SendAsync(message, token);
        var jsonStr = await reponse.Content.ReadAsStringAsync(token);
        var wallpaper =  JsonSerializer.Deserialize<BingWallpaper>(jsonStr);
        if (wallpaper == null) return null;
        return wallpaper.Images[0];
    }
}
