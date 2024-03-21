namespace Aria2.Client.Models.Messagers;

/// <summary>
/// 切换任务视图，比如说活动中到停止
/// </summary>
public class TellTaskSwitchMessager
{
    public TellTaskSwitchMessager(string oldStatus, string newStatus)
    {
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }

    public string OldStatus { get; }
    public string NewStatus { get; }
}
