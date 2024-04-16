using Aria2.Client.Models;
using Aria2.Client.Models.Anime;
using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels.FrameViewModels;

public sealed partial class AnimeViewModel : ObservableRecipient
{
    public AnimeViewModel(IOnekumaService rssService, IDataFactory dataFactory)
    {
        RssService = rssService;
        DataFactory = dataFactory;
        Resources.CollectionChanged += Resources_CollectionChanged;
        this.CardVisibility = 0;
    }


    public IOnekumaService RssService { get; }
    public IDataFactory DataFactory { get; }

    [ObservableProperty]
    ObservableCollection<AnimeItemData> _Resources = new();

    [ObservableProperty]
    ObservableCollection<string> _SearchTypes =
        new()
        {
            "默认",
            "動畫",
            "季度全集",
            "漫畫",
            "港台原版",
            "日文原版",
            "音樂",
            "動漫音樂",
            "同人音樂",
            "流行音樂",
            "日劇",
            "ＲＡＷ",
            "遊戲",
            "電腦遊戲",
            "電視遊戲",
            "掌機遊戲",
            "網絡遊戲",
            "遊戲周邊",
            "特攝",
        };

    [ObservableProperty]
    ObservableCollection<string> _Fansubs = new();

    [ObservableProperty]
    string _Fansub = "默认";

    [ObservableProperty]
    string _SelectType = "默认";

    [ObservableProperty]
    string _Fliter;

    partial void OnSelectTypeChanged(string value)
    {
        switch (value)
        {
            case "動畫":
                this.Fansubs = new()
                {
                    "默认",
                    "桜都字幕组",
                    "北宇治字幕组",
                    "极影字幕社",
                    "喵萌奶茶屋",
                    "悠哈C9字幕社",
                    "LoliHouse",
                    "Lilith-Raws",
                    "天月動漫&發佈組",
                    "千夏字幕组",
                    "ANi",
                    "MingYSub",
                    "SweetSub",
                    "爱恋字幕社",
                    "动漫国字幕组",
                    "幻樱字幕组",
                    "天使动漫论坛",
                    "霜庭云花Sub",
                    "星空字幕组",
                    "轻之国度",
                    "MCE汉化组",
                    "c.c动漫",
                    "诸神kamigami字幕组",
                    "白恋字幕组",
                    "氢气烤肉架",
                    "六道我大鸽汉化组",
                    "云歌字幕组",
                    "成子坂地下室",
                    "失眠搬运组",
                    "SRVFI-Raws",
                    "PharosofMyGO",
                    "拨雪寻春",
                };
                break;
            case "特攝":
                this.Fansubs = new() { "默认", "魔星字幕团", "DBD制作组", "KRL字幕组", };
                break;
            case "日劇":
                this.Fansubs = new()
                {
                    "默认",
                    "幻月字幕组",
                    "云光字幕组",
                    "豌豆字幕组",
                    "驯兽师联盟",
                    "中肯字幕組",
                    "SW字幕组",
                    "风之圣殿",
                    "华盟字幕社",
                    "波洛咖啡厅",
                    "动音漫影",
                    "VCB-Studio",
                    "DHR動研字幕組",
                    "80v08",
                    "肥猫压制",
                    "Little字幕组",
                    "AI-Raws",
                    "离谱Sub",
                    "虹咲学园烤肉同好会",
                    "ARIA吧汉化组",
                    "百冬練習組",
                    "冷番补完字幕组",
                    "爱咕字幕组",
                    "極彩字幕组",
                    "未央阁联盟",
                    "届恋字幕组",
                    "夜莺家族",
                    "TD-RAWS",
                    "夢幻戀櫻",
                    "WBX-SUB",
                    "Amor字幕组"
                };
                break;
            default:
                this.Fansubs.Clear();
                break;
        }
    }

    [ObservableProperty]
    double _CardVisibility = 0;

    [ObservableProperty]
    string _Query;

    private void Resources_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (Resources.Count == 0)
            this.CardVisibility = 1;
        else
            this.CardVisibility = 0;
    }

    [RelayCommand]
    async Task SearchAsync()
    {
        this.Resources.Clear();
        var result = await RssService.SearchKeyworkd(
            new List<string>() { Query },
            Fliter == null ? null: this.Fliter.Split(' ').ToList(),
            SelectType == "默认" ? null : SelectType,
            this.Fansub == "默认" ? null : this.Fansub
        );
        if (result == null)
        {
            this.CardVisibility = 0;
            return;
        }
        foreach (var item in result.Resources)
        {
            this.Resources.Add(DataFactory.CreateAnimeItemData(item));
        }
    }
}
