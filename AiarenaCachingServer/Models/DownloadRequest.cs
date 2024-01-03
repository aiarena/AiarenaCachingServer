using System.Text.Json.Serialization;

namespace AiArenaCachingServer.Controllers;

public class DownloadRequest
{
    [JsonPropertyName("uniqueKey")]
    public string UniqueKey { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    [JsonPropertyName("md5hash")]
    public string Md5Hash { get; set; }
}