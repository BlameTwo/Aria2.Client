using System.Net.WebSockets;

namespace Aria2.Net.Models.Handler;

public delegate void Aria2ConnectStateChangedDelegate(object source,WebSocketState state);

public delegate void Aria2WebSocketMessageDelegate(object source, WebSocketResultCode result);


#region Aria2下载动作
public delegate void Aria2DownloadStartDelegate(object source, WebSocketResultCode result);

public delegate void Aria2DownloadCompleteDelegate(object source, WebSocketResultCode result);

public delegate void Aria2DownloadPauseDelegate(object source,WebSocketResultCode result);

public delegate void Aria2DownloadStopDelegate(object source,WebSocketResultCode stop);

public delegate void Aria2DownloadErrorDelegate(object source, WebSocketResultCode error);

public delegate void Aria2DownloadBtCompleteDelegate(object source,WebSocketResultCode error);
#endregion
