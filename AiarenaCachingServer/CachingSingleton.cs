using System.Collections.Concurrent;

namespace AiArenaCachingServer.Controllers;

public class CachingSingleton
{
    public ConcurrentDictionary<string, CacheObject> CachingMap { get; set; } = new();
}