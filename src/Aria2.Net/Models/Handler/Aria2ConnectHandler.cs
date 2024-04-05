using Aria2.Net.Enums;
using System.Net.WebSockets;

namespace Aria2.Net.Models.Handler;

public delegate void Aria2ConnectStateChangedDelegate(object source,WebSocketState state);

public delegate void Aria2WebSocketMessageDelegate(object source, WebSocketResultCode result);


#region Aria2下载动作
public delegate void Aria2DownloadStateChangedDelegate(WebSocketEventType @eventType,WebSocketResultCode state);
#endregion
