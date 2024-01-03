using System.Text.Json.Serialization;

namespace AiArenaCachingServer.Models;

public class AiArenaBot
{
    [JsonPropertyName("id")]
    public uint Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("game_display_id")]
    public string GameDisplayId { get; set; }
    [JsonPropertyName("bot_zip")]
    public string BotZip { get; set; }
    [JsonPropertyName("bot_zip_md5hash")]
    public string BotZipMd5Hash { get; set; }
    [JsonPropertyName("bot_data")]
    public string BotData { get; set; }
    [JsonPropertyName("bot_data_md5hash")]
    public string BotDataMd5Hash { get; set; }
    [JsonPropertyName("plays_race")]
    public string PlaysRace { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
}