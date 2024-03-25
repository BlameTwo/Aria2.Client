using Aria2.Net.Models;
using Aria2.Net.Models.ClientModel;
using Aria2.Net.Models.Enums;
using Aria2.Net.Models.Handler;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Aria2.Net.Common;

namespace Aria2.Net.Services.Contracts;

public interface IAria2cClient
{
    /// <summary>
    /// Socket 连接
    /// </summary>
    public ClientWebSocket Socket { get; }

    /// <summary>
    /// 启动Aria2服务
    /// </summary>
    /// <param name="config">带配置启动</param>
    /// <returns></returns>
    public Task LauncherAsync(Aria2Config config);
    
    public Task LauncherAsync();

    /// <summary>
    /// 添加下载链接
    /// </summary>
    /// <param name="uriList">下载列表</param>
    /// <param name="options">配置</param>
    /// <param name="location">位置</param>
    /// <param name="token">令牌</param>
    /// <returns></returns>
    public Task<ResultCode<string>> AddUriAsync(IEnumerable<string> uriList,IDictionary<string,object> options,int? location = null,CancellationToken token = default);

    /// <summary>
    /// 添加BT文件下载
    /// </summary>
    /// <param name="torrentPath">BT文件位置</param>
    /// <param name="options">配置</param>
    /// <param name="location">位置</param>
    /// <param name="token">令牌</param>
    /// <returns></returns>
    public Task<ResultCode<string>> AddTorrentAsync(string torrentPath, IDictionary<string, object> options, int? location = null, CancellationToken token = default);
    
    /// <summary>
    /// 设置aria2全局设置
    /// </summary>
    /// <param name="querys"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> ChangeGlobalOption(IDictionary<string,string> querys,CancellationToken token = default);

    /// <summary>
    /// 获得aria2全局设置
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<Aria2cOption>> GetGlobalOption(CancellationToken token = default);

    /// <summary>
    /// 获得正在活动的任务列表
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<FileDownloadTell>>> GetAllTellActiveAsync(CancellationToken token = default);

    public Task<WebSocketState> ReconnectionSocket(CancellationToken token = default);

    /// <summary>
    /// 根据gid查找任务
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<FileDownloadTell>> GetTellStatusAsync(string gid, CancellationToken token = default);

    /// <summary>
    /// 暂停所有任务
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> PaushAllTask(CancellationToken token = default);

    /// <summary>
    /// 暂停一个任务
    /// </summary>
    /// <param name="gid">任务gid</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> PauseTask(string gid,CancellationToken token = default);

    /// <summary>
    /// 移除一个任务
    /// </summary>
    /// <param name="gid">gid</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> RemoveTask(string gid,CancellationToken token = default);

    /// <summary>
    /// 退出aria2
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task ExitAria2(CancellationToken token = default);

    /// <summary>
    /// 立即暂停一个下载
    /// </summary>
    /// <param name="gid">gid</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<string>>> ForcePaush(string gid, CancellationToken token = default);

    /// <summary>
    /// 立即暂停所有下载
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<string>>> ForcePauseAll(CancellationToken token = default);

    /// <summary>
    /// 设置aria2全局设置
    /// </summary>
    /// <param name="type">设置枚举</param>
    /// <param name="value">值</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> ChangGlobalOption(Aria2GlobalOptionEnum type, object value,CancellationToken token = default);

    /// <summary>
    /// 设置的aria2全局设置
    /// </summary>
    /// <param name="keyvalues">多个设置项目</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<bool> ChangGlobalOptions(Dictionary<Aria2GlobalOptionEnum, object> keyvalues, CancellationToken token = default);

    /// <summary>
    /// 恢复全部下载到活动任务
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> UnpauseAll(CancellationToken token = default);

    /// <summary>
    /// 恢复一个下载
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> Unpause(string gid, CancellationToken token = default);

    /// <summary>
    /// 获得一个任务的设置
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<TellOption>> GetTellOption(string gid, CancellationToken token = default);

    /// <summary>
    /// 获得整体下载速度
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<AllTellStatus>> GetAllTellStatus(CancellationToken token = default);

    /// <summary>
    /// 获得正在等待的任务
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="pagesize"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<FileDownloadTell>>> GetWaitingTaskAsync(int offset = 0, int pagesize = 1000, CancellationToken token = default);

    /// <summary>
    /// 获得一个已经终止的任务
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="pagesize"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<FileDownloadTell>>> GetStopedTaskAsync(int offset = 0, int pagesize = 1000, CancellationToken token = default);

    /// <summary>
    /// 连接WebSocket
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<WebSocketState> ConnectAsync(CancellationToken token=default);

    /// <summary>
    /// 断开WebSocket连接
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<Tuple<WebSocketState,WebSocketCloseStatus?>> DisconnectAsync(CancellationToken token=default);

    /// <summary>
    /// 连接状态更改
    /// </summary>
    public event Aria2ConnectStateChangedDelegate Aria2ConnectStateChanged;

    /// <summary>
    /// WebSocket 消息事件
    /// </summary>
    public event Aria2WebSocketMessageDelegate Aria2WebSocketMessage;

    /// <summary>
    /// 移除指定任务
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> Aria2RemoveDownloadResult(string gid,CancellationToken token = default);


    public Task<ResultCode<string>> Aria2RemoveAllDownloadResult(CancellationToken token = default);

    public Task<ResultCode<List<DownloadFile>>> GetFiles(string gid, CancellationToken token = default);

    public Task<ResultCode<string>> ChangeTellOption(string gid,Dictionary<string,object> values,CancellationToken token = default);



    public event Aria2DownloadStateChangedDelegate Aria2DownloadStateEvent;
}