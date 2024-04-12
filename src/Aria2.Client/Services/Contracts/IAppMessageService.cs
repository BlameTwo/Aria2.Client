using Aria2.Client.Models.Enums;
using System;

namespace Aria2.Client.Services.Contracts;

public interface IAppMessageService
{
    string SendMessage(string message, string title, MessageLevel level, bool IsClear);

    void ClearMessage(string guid);



    public string SendTimeSpanMessage(TimeSpan time, string message, string title, MessageLevel level);
}