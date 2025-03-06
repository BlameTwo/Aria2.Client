using Aria2.Client.Models.Enums;
using System;

namespace Aria2.Client.Models.Messagers;

public class AppNotifyMessager
{
    public TimeSpan DelayTime { get; set; }
    public AppNotifyMessager(string message, string title, MessageLevel level,bool IsClear)
    {
        Message = message; 
        Title = title;
        Level = level;
        this.IsClear = IsClear;
        Guid = System.Guid.NewGuid().ToString("N");
    }

    public AppNotifyMessager(string message, string title, MessageLevel level, bool IsClear,TimeSpan time)
    {
        Message = message;
        Title = title;
        Level = level;
        this.IsClear = IsClear;
        Guid = System.Guid.NewGuid().ToString("N");
        DelayTime = time;
    }


    public string Message { get; }
    public string Title  {get;}
    public MessageLevel Level { get; }
    public bool IsClear { get; }
    public string Guid { get; }
}

public class AppClearNotifyMessager(string guid)
{
    public string Guid { get; } = guid;
}
