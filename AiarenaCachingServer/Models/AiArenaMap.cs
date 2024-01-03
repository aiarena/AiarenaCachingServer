using System.Text.Json.Serialization;

namespace AiArenaCachingServer.Models;

public class AiArenaMap
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("file")]
    public string File { get; set; }
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
    [JsonPropertyName("GameMode")]
    public long GameMode { get; set; }
    [JsonPropertyName("competitions")]
    public List<long> Competitions { get; set; } 
}