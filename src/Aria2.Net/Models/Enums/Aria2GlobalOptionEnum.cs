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

public static class GlobalOptionConvert
{
    public static string EnumToString(Aria2GlobalOptionEnum aria2)
    {
        switch (aria2)
        {
            case Aria2GlobalOptionEnum.BtMaxFile:
                return "bt-max-open-files";
            case Aria2GlobalOptionEnum.SaveDownloadResult:
                return "keep-unfinished-download-result";
            case Aria2GlobalOptionEnum.LogFilePath:
                return "log";
            case Aria2GlobalOptionEnum.LogLevel:
                return "log-level";
            case Aria2GlobalOptionEnum.MaxConcurrentDownloadCount:
                return "max-concurrent-downloads";
            case Aria2GlobalOptionEnum.MaxDownloadSaveResultCount:
                return "max-download-result";
            case Aria2GlobalOptionEnum.MaxAllDownloadLimit:
                return "max-overall-download-limit";
            case Aria2GlobalOptionEnum.MaxAllUploadLimit:
                return "max-overall-upload-limit";
            case Aria2GlobalOptionEnum.OptimizeConCurrent:
                return "optimize-concurrent-downloads";
            case Aria2GlobalOptionEnum.CookiePath:
                return "save-cookies";
            case Aria2GlobalOptionEnum.SaveSession:
                return "save-session";
            default:
                return null;
        }
    }
}
