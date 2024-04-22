using System.Text.Json.Serialization;

namespace BtSearch.Loader.Models;

public  class PluginInstallerVersion
{
    [JsonPropertyName("Version")]
    public string Version { get; set; }

    [JsonPropertyName("Guid")]
    public string Guid { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }
}
