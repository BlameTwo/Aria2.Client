using System.Text.Json.Serialization;

namespace Aria2.Net.Models;

public class AppOptions
{
    [JsonPropertyName("downloadPath")]
    public string DownloadPath { get; set; }
}
