namespace AiArenaCachingServer.Controllers;

public class CachingSingleton
{
    public Dictionary<string, CacheObject> CachingMap { get; set; } = new();
}