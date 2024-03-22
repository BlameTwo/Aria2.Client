﻿using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using Aria2.Net.Models.ClientModel;
using System.Collections.Generic;

namespace Aria2.Client.Services;

public class DataFactory : IDataFactory
{
    public Object CreateItemData<Object, Value>(Value value)
        where Object : class, IItemData<Value>
    {
        var item = ProgramLife.GetService<Object>();
        item.SetData(value);
        return item;
    }

    public List<Object> CreateItemDatas<Object, Value>(List<Value> value)
        where Object : class, IItemData<Value>
    {
        List<Object> items = new();
        foreach (var item in value)
        {
            items.Add(CreateItemData<Object, Value>(item));
        }
        return items;
    }

    public DownloadTellItemData CreateownloadTellItemData(FileDownloadTell tellValue)
    {
        return CreateItemData<DownloadTellItemData, FileDownloadTell>(tellValue);
    }

    public List<DownloadTellItemData> CreateownloadTellItemDatas(List<FileDownloadTell> tellValue)
    {
        return CreateItemDatas<DownloadTellItemData, FileDownloadTell>(tellValue);
    }
}