using Aria2.Client.Models.Messagers;
using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Aria2.Client.Models.Bases;

public partial class ItemDownloadBase<T> :ObservableRecipient ,IItemData<T>, IRecipient<TellTaskSwitchMessager>
{
    [ObservableProperty]
    T _Data;

    public void Receive(TellTaskSwitchMessager message)
    {
        MessageReceive(message);
    }

    public virtual void MessageReceive(TellTaskSwitchMessager message)
    {

    }

    public virtual void SetData(T data)
    {
        IsActive = true;
        Data = data;
    }
}
