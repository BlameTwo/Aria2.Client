namespace Aria2.Net;

public static class GlobalUsings
{
    public const string Aria2Path = "C:\\Users\\30140\\Desktop\\aria2-1.37.0-win-64bit-build1\\aria2c.exe";

    public const string Port = "5050";

    public const string HttpRequetBaseUrl = $"http://127.0.0.1:{Port}/jsonrpc";

    public const string WebSocketBaseUrl = $"ws://localhost:5050/jsonrpc";

    public const string RequestOK = "OK";


    public const string AddUri_Method = "aria2.addUri";

    public const string AddTorrent_Method = "aria2.addTorrent";

    public const string ChangeGlobalOption_Method = "aria2.changeGlobalOption";

    public const string GetGlobalOption_Method = "aria2.getGlobalOption";

    public const string GetTellStatus_Method = "aria2.tellStatus";

    public const string GetTellActive_Method = "aria2.tellActive";

    public const string PauseAll_Method = "aria2.pauseAll";

    public const string Pause_Method = "aria2.pause";

    public const string ForcePause = "aria2.forcePause";

    public const string ForcePauseAll = "aria2.forcePauseAll";

    public const string ForceRemove = "aria2.forceRemove";
}

public static class Aria2Socket_Method
{
    public const string OnDowloadStart = "aria2.onDownloadStart";

    public const string OnDownloadPause = "aria2.onDownloadPause";

    public const string OnDownloadStop = "aria2.onDownloadStop";

    public const string OnDownloadComplete = "aria2.onDownloadComplete";

    public const string OnDowloadError = "aria2.onDownloadError";

    public const string OnBtDownloadComplete = "aria2.onBtDownloadComplete";
}

public static class TellState
{
    public const string Active = "active";

    public const string Waiting = "waiting";

    public const string Paused = "paused";

    public const string Error = "error";

    public const string Complete = "complete";

    public const string Removed = "removed";

    public const string Stopped = "stopped";
}