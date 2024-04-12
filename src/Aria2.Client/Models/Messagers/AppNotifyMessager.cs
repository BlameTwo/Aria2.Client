using Aria2.Client.Models.Enums;
using Microsoft.UI.Xaml;
using System;

namespace Aria2.Client.Models.Messagers;

public class AppNotifyMessager
{
    public TimeSpan DelayTime { get; set; }
    public AppNotifyMessager(string message, string title, MessageLevel level,bool IsClear)
    {
        this.Message = message; 
        this.Title = title;
        this.Level = level;
        this.IsClear = IsClear;
        this.Guid = System.Guid.NewGuid().ToString("N");
    }

    public AppNotifyMessager(string message, string title, MessageLevel level, bool IsClear,TimeSpan time)
    {
        this.Message = message;
        this.Title = title;
        this.Level = level;
        this.IsClear = IsClear;
        this.Guid = System.Guid.NewGuid().ToString("N");
        this.DelayTime = time;
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
