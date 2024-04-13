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
        this.Client = new();
    }

    public async Task<YurikotoModel> GetYurikoWallpaper(CancellationToken canceltoken = default)
    {
        HttpRequestMessage message = new(HttpMethod.Get, "https://v1.yurikoto.com/wallpaper?encode=json");
        var reponse = await Client.SendAsync(message, canceltoken);
        var jsonStr = await reponse.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<YurikotoModel>(jsonStr);
    }
}
