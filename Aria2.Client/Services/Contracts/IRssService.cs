
using Aria2.Client.Models.Anime;
using System.Threading.Tasks;

namespace Aria2.Client.Services.Contracts;

public interface IRssService
{
    public void AddUrl(string url);


    public Task<AnimeTorrentModel> GetAnimeHomeAsync(int page,int pagesize);
}