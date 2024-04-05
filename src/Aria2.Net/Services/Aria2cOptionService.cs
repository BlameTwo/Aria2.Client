using Aria2.Net.Contracts;
using Aria2.Net.Models;
using Aria2.Net.Services.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.Net.Services;

public class Aria2cOptionService : IAria2cOptionService
{
    public Aria2cOptionService(IAria2cClient aria2CClient)
    {
        Aria2CClient = aria2CClient;
    }

    public IAria2cClient Aria2CClient { get; }

    public async Task<ResultCode<string>> ChangeMaxDownloadAsync(string value, CancellationToken token)
    {
        return await Aria2CClient.ChangGlobalOption(Models.Enums.Aria2GlobalOptionEnum.MaxAllDownloadLimit, value,token);
    }

    public async Task<ResultCode<string>> ChangeMaxUploadAsync(string value, CancellationToken token)
    {
        return await Aria2CClient.ChangGlobalOption(Models.Enums.Aria2GlobalOptionEnum.MaxAllUploadLimit, value, token);
    }
}