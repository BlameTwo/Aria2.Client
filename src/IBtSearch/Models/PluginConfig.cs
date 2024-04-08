using System;
using System.Text.Json.Serialization;

namespace IBtSearch.Models;

public class PluginConfig
{
    [JsonPropertyName("IsEnabled")]
    public bool IsEnabled 
    { 
        get; 
        set; 
    }

    [JsonPropertyName("LastRunTime")]
    public DateTime LastActive { get; set; }

    
}