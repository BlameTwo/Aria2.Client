using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aria2.Net.Models.ClientModel;

public class DownloadFile
{
    [JsonPropertyName("completedLength")]
    public string CompletedLength { get; set; }

    [JsonPropertyName("index")]
    public string Index { get; set; }

    [JsonPropertyName("length")]
    public string Length { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("selected")]
    public string Selected { get; set; }

    [JsonPropertyName("uris")]
    public List<object> Uris { get; set; }
}