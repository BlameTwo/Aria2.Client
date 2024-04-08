using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aria2.Net.Common;

public sealed class Aria2LauncherConfig
{
    [JsonPropertyName("SessionFile")]
    public string SesionFilePath { get; set; }

    [JsonPropertyName("BTTrackers")]
    public List<string> BtTracker { get; set; }

    [JsonPropertyName("LogFile")]
    public string LogFilePath { get; set; }

    [JsonPropertyName("MaxDownloadSpeed")]
    public string MaxDownloadSpeed { get; set; }

    [JsonPropertyName("MaxUploadSpeed")]
    public string MaxUploadSpeed { get; set; }

    [JsonPropertyName("MaxSaveResult")]
    public int MaxSaveResultCount { get; set; }
}
