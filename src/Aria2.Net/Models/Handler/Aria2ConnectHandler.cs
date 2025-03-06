using Aria2.Net.Enums;
using System.Net.WebSockets;

namespace Aria2.Net.Models.Handler;

public delegate void Aria2ConnectStateChangedEventHandler(object source, WebSocketState state);

public delegate void Aria2WebSocketMessageEventHandler(object source, WebSocketResultCode result);

#region Aria2下载动作
public delegate void Aria2DownloadStateChangedEventHandler(WebSocketEventType eventType, WebSocketResultCode state);
#endregion
