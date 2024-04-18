
using Aria2.Client.Models.Anime;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Client.Services.Contracts;

public interface IOnekumaService
{
    public void AddUrl(string url);


    public Task<AnimeTorrentModel> GetAnimeHomeAsync(int page,int pagesize);

    public Task<AnimeTorrentModel> SearchKeyWord(List<string> keyword, List<string> fliter = null, string Type = null, string Fansub = null, CancellationToken token = default, int page = 1, int pageSize = 20);

}