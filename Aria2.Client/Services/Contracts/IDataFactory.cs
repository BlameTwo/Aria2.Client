using Aria2.Client.Models;
using Aria2.Net.Models.ClientModel;
using System.Collections.Generic;

namespace Aria2.Client.Services.Contracts;

public interface IDataFactory
{
    public Object CreateItemData<Object, Value>(Value value)
        where Object : class, IItemData<Value>;

    public List<Object> CreateItemDatas<Object, Value>(List<Value> value)
        where Object : class, IItemData<Value>;

    public DownloadTellItemData CreateownloadTellItemData(FileDownloadTell tellValue);

    public List<DownloadTellItemData> CreateownloadTellItemDatas(List<FileDownloadTell> tellValue);
}
