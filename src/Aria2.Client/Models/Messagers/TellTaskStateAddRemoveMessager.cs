using CommunityToolkit.Mvvm.ComponentModel;

namespace Aria2.Client.Models.Messagers;

public partial class TellTaskStateAddRemoveMessager:ObservableRecipient
{
    public TellTaskStateAddRemoveMessager(string orginState,string nowState,string gid)
    {
        OrginState = orginState;
        NowState = nowState;
        Gid = gid;
    }

    [ObservableProperty]
    string _OrginState;

    [ObservableProperty]
    string _NowState;

    [ObservableProperty]
    string _Gid;
}

public partial class TellTaskStateAddRemoveItemMessager
{
    public TellTaskStateAddRemoveItemMessager(DownloadTellItemData value,bool isRemove,bool isAdd)
    {
        Value = value;
        IsRemove = isRemove;
        IsAdd = isAdd;
    }

    public DownloadTellItemData Value { get; }
    public bool IsRemove { get; }
    public bool IsAdd { get; }
}
