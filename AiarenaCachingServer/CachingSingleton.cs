using System.Collections.Concurrent;
using AiarenaCachingServer.Models;

namespace AiarenaCachingServer;

public class CachingSingleton
{
    public ConcurrentDictionary<string, CacheObject> CachingMap { get; set; } = new();
}