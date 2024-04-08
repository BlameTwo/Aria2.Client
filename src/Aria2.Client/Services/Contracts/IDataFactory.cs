using Aria2.Client.Models;
using Aria2.Client.Models.Anime;
using Aria2.Net.Models.ClientModel;
using IBtSearch.Bases;
using IBtSearch.Models;
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

    public AnimeItemData CreateAnimeItemData(AnimeResource value);

    public List<AnimeItemData> CreateAnimeItemDatas(List<AnimeResource> values);

    public BTSearchRresultItem CreateBTSearchRresultItem(BTSearchResult value);

    public BTPluginItemData CreatePluginItem(IAria2Plugin value);
}
