using Aria2.Client.Models;
using System.Collections.Generic;

namespace Aria2.Client.Services.Contracts;

public interface IDataAcquistionService
{
    public List<DownloadSave> CacheData { get; set; }
}
