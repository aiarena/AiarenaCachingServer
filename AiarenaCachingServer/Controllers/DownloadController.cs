﻿using AiarenaCachingServer;
using AiarenaCachingServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AiArenaCachingServer.Controllers;

[ApiController]
[Route("download")]
public class DownloadController(
    HttpClient httpClient,
    CachingSingleton cachingSingleton,
    IConfiguration configuration,
    ILogger<DownloadController> logger) : Controller
{
    private string AuthHeader()
    {
        return $"Token {configuration.GetValue<string>("ApiToken")}";
    }

    [HttpPost]
    public async Task<IActionResult> Download([FromBody] DownloadRequest downloadRequest)
    {
        logger.LogInformation("Received request: {UniqueKey}, {Md5Hash}", downloadRequest.UniqueKey,
            downloadRequest.Md5Hash);
        if (cachingSingleton.CachingMap.TryGetValue(downloadRequest.UniqueKey, out CacheObject? cacheObject) &&
            cacheObject.Md5Hash == downloadRequest.Md5Hash && Path.Exists(cacheObject.Path))
        {
            logger.LogInformation("Cache exists for {UniqueKey}, {Md5Hash}", downloadRequest.UniqueKey,
                downloadRequest.Md5Hash);
            var fileStream = System.IO.File.OpenRead(cacheObject.Path);
            return new FileStreamResult(fileStream, "application/octet-stream");
        }

        logger.LogInformation("Cache does not exist for {UniqueKey}, {Md5Hash}", downloadRequest.UniqueKey,
            downloadRequest.Md5Hash);
        if (cacheObject != null && Path.Exists(cacheObject.Path))
        {
            System.IO.File.Delete(cacheObject.Path);
        }

        var url = downloadRequest.Url;
        httpClient.DefaultRequestHeaders.Remove("Authorization");
        if (url.Contains("aiarena.net"))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", AuthHeader());
        }

        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadAsStringAsync();
            logger.LogError("Error(AiArena): Response: {ResponseMessage}\nStatus:{StatusCode}", responseMessage,
                response.StatusCode);
        }

        response.EnsureSuccessStatusCode();


        var downloadDirectory = configuration.GetValue<string>("DownloadDirectory")!;

        Directory.CreateDirectory(downloadDirectory);

        var newPath = Path.Join(downloadDirectory, downloadRequest.Md5Hash);

        var file = await response.Content.ReadAsStreamAsync();

        await using (var fileStream = System.IO.File.Create(newPath))
        {
            file.Seek(0, SeekOrigin.Begin);
            await file.CopyToAsync(fileStream);
            fileStream.Close();
        }

        var newCacheObject = new CacheObject()
        {
            Md5Hash = downloadRequest.Md5Hash,
            Path = newPath
        };
        
        cachingSingleton.CachingMap.AddOrUpdate(downloadRequest.UniqueKey, newCacheObject, (_, _) => newCacheObject);


        file.Seek(0, SeekOrigin.Begin);
        return new FileStreamResult(file, "application/octet-stream");
    }
}