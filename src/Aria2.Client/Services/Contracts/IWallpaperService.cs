using System.Threading.Tasks;
using System.Threading;
using Aria2.Client.Models.Wallpaper;

namespace Aria2.Client.Services.Contracts;

public interface IWallpaperService
{
    public Task<YurikotoModel> GetYurikoWallpaper(CancellationToken canceltoken = default);


    public Task<BingImage> GetSingleBingWallpaper(CancellationToken token = default);
}
