using CommunityToolkit.Mvvm.Input;
using IBtSearch.Models;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IBtSearch.Bases;

public interface IAria2Plugin
{
    [JsonPropertyName("Guid")]
    public string Guid { get; }

    [JsonPropertyName("Name")]
    public string Name { get; }

    [JsonPropertyName("IconWebUrl")]
    public string Icon { get; }

    [JsonPropertyName("Version")]
    public string Version { get; }

    [JsonPropertyName("Config")]
    PluginConfig Config { get; set; }

    public Task LoadConfig(string folderPath);

    public Task SetEnabledAsync(); 
}
