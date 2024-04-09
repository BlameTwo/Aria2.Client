using Aria2.Client.Models.Enums;

namespace Aria2.Client.Services.Contracts;

public interface IAppMessageService
{
    void SendMessage(string message, string title, MessageLevel level);
}
