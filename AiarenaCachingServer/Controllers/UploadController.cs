using System.Security.Cryptography;
using AiarenaCachingServer;
using AiarenaCachingServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AiArenaCachingServer.Controllers;

[ApiController]
public class UploadController(
    CachingSingleton cachingSingleton,
    IConfiguration configuration, ILogger<UploadController> logger) : Controller
{
    [HttpPost]
    [Route("upload")]
    [RequestSizeLimit(long.MaxValue)]
    [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
    public async Task<IActionResult> Upload(IFormFile file, string uniqueKey)
    {
        logger.LogInformation("Upload request received for {UniqueKey}", uniqueKey);
        var stream = file.OpenReadStream();
        var md5Hash = ComputeMd5Hash(stream);
        logger.LogInformation("Calculated md5 hash for {UniqueKey}: {Md5Hash}", uniqueKey, md5Hash);
        
        
        if (cachingSingleton.CachingMap.TryGetValue(uniqueKey, out var cObject) )
        {
            if (cObject.Md5Hash == md5Hash)
            {
                return Ok();
            }
            if (Path.Exists(cObject.Path))
            {
                System.IO.File.Delete(cObject.Path);
            }
        }
        
        
        var downloadDirectory = configuration.GetValue<string>("DownloadDirectory")!;


        Directory.CreateDirectory(downloadDirectory);
 
        var newPath = Path.Join(downloadDirectory, md5Hash);


        await using (var fileStream = System.IO.File.Create(newPath))
        {
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fileStream);
            fileStream.Close();
        }

        var newCacheObject = new CacheObject()
        {
            Md5Hash = md5Hash,
            Path = newPath
        };

        cachingSingleton.CachingMap.AddOrUpdate(uniqueKey, newCacheObject, (_, _) => newCacheObject);

        return Ok();
    }

    private static string ComputeMd5Hash(Stream stream)
    {
        using var md5 = MD5.Create();
        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
    }

    [HttpPost]
    [Route("hash")]
    [RequestSizeLimit(long.MaxValue)]
    public IActionResult CalculateHash(IFormFile file)
    {
        var stream = file.OpenReadStream();
        return Ok(ComputeMd5Hash(stream));
    }
}