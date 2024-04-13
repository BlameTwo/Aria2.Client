using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Aria2.Client.Models.Wallpaper;

public record YurikotoModel(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("link")] string Link,
    [property: JsonPropertyName("orientation")] string Orientation,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("type")] string Type
);