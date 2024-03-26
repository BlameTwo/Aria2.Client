using Aria2.Client.Models.Anime;
using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace Aria2.Client.Models;

public sealed partial class AnimeItemData : ObservableRecipient,IItemData<AnimeResource>
{
    [ObservableProperty]
    AnimeResource _Data;

    public void SetData(AnimeResource data)
    {
        this.Data = data;
        DataRefresh();
    }

    private void DataRefresh()
    {

    }


    [RelayCommand]
    async Task AddDownload()
    {

    }
}
