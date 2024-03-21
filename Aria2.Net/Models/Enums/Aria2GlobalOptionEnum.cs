using Aria2.Net.Models.Attributes;

namespace Aria2.Net.Models.Enums;

public enum Aria2GlobalOptionEnum
{
    [GlobalOptionProperty(display:"最大Bt文件数量", "bt-max-open-files")]
    BtMaxFile,
    [GlobalOptionProperty(display:"保留下载任务到会话中", "keep-unfinished-download-result")]
    SaveDownloadResult,
    [GlobalOptionProperty(display:"日志文件名称","log")]
    LogFilePath,
    [GlobalOptionProperty(display: "日志级别：debug info notice warn error debug","log-level")]
    LogLevel,
    [GlobalOptionProperty(display:"最大同时下载数量", "max-concurrent-downloads")]
    MaxConcurrentDownloadCount,
    [GlobalOptionProperty(display:"最大保存的下载结果数量", "max-download-result")]
    MaxDownloadSaveResultCount,
    [GlobalOptionProperty(display:"最大下载速度", "max-overall-download-limit")]
    MaxAllDownloadLimit,
    [GlobalOptionProperty(display:"最大上传速度", "max-overall-upload-limit")]
    MaxAllUploadLimit,
    [GlobalOptionProperty(display:"并发优化下载", "optimize-concurrent-downloads")]
    OptimizeConCurrent,
    [GlobalOptionProperty(display:"保存Cookie地址", "save-cookies")]
    CookiePath,
    [GlobalOptionProperty(display:"保存会话地址", "save-session")]
    SaveSession,
}
