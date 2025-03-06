using Aria2.Client.Models.Enums;
using Aria2.Client.Models.Messagers;
using Aria2.Client.Services.Contracts;
using CommunityToolkit.Mvvm.Messaging;
using System;

namespace Aria2.Client.Services;

public class AppMessageService : IAppMessageService
{
    public void ClearMessage(string guid)
    {
        WeakReferenceMessenger.Default.Send<AppClearNotifyMessager>(new(guid));
    }

    public string SendMessage(string message, string title, MessageLevel level,bool IsClear)
    {
        Tuple<bool, AppNotifyMessager> result = new(true, new(message, title, level,IsClear));
        WeakReferenceMessenger.Default.Send<Tuple<bool, AppNotifyMessager>>(
            result
        );
        return result.Item2.Guid;
    }


    public string SendTimeSpanMessage(TimeSpan time, string message, string title, MessageLevel level)
    {
        Tuple<bool, AppNotifyMessager> result = new(true, new(message, title, level, false, time));
        WeakReferenceMessenger.Default.Send<Tuple<bool, AppNotifyMessager>>(
            result
        );
        return result.Item2.Guid;
    }
}
