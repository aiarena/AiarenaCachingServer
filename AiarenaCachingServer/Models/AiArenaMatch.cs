using System.Text.Json.Serialization;

namespace AiArenaCachingServer.Models;

public class AiArenaMatch
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }
    [JsonPropertyName("bot1")]
    public AiArenaBot Bot1 { get; set; }
    [JsonPropertyName("bot2")]
    public AiArenaBot Bot2 { get; set; }
    [JsonPropertyName("map")]
    public AiArenaMap Map { get; set; }
}