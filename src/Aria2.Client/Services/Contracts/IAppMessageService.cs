using Aria2.Client.Models.Enums;

namespace Aria2.Client.Services.Contracts;

public interface IAppMessageService
{
    string SendMessage(string message, string title, MessageLevel level, bool IsClear);

    void ClearMessage(string guid);
}