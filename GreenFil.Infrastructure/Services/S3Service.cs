using Amazon.S3;
using Amazon.S3.Transfer;
using GreenFil.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GreenFil.Infrastructure.Services;

public class S3Service : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    public string BucketName => "greenfil-modelos3d";

    public S3Service(IConfiguration configuration)
    {
        var accessKey = configuration["AWS:AccessKey"];
        var secretKey = configuration["AWS:SecretKey"];
        var region = configuration["AWS:Region"];

        var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
        _s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
    }

    public async Task<string> GuardarImagenAsync(IFormFile archivo, string subcarpeta)
    {
        var extension = Path.GetExtension(archivo.FileName);
        var nombreBase = Path.GetFileNameWithoutExtension(archivo.FileName).Replace(" ", "_").ToLower();
        var nombreArchivo = $"{subcarpeta}/{nombreBase}{extension}";
        
        var tempPath = Path.GetTempFileName();
        await using (var stream = File.Create(tempPath))
        {
            await archivo.CopyToAsync(stream);
        }

        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(tempPath, BucketName, nombreArchivo);
        File.Delete(tempPath);

        return $"https://{BucketName}.s3.amazonaws.com/{nombreArchivo}";
    }
}