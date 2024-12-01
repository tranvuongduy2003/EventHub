using EventHub.Abstractions.Services;
using EventHub.Shared.DTOs.File;
using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Serilog;

namespace EventHub.Infrastructure.FilesSystem;

public class MinioFileService : IFileService
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger _logger;

    public MinioFileService(IMinioClient minioClient, ILogger logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Uri> GetUriByFileNameAsync(string bucketName, string objectName)
    {
        string presignedUrl = await _minioClient.PresignedGetObjectAsync(
            new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithExpiry(DateTimeOffset.UtcNow.AddDays(1).Microsecond));
        return new Uri(presignedUrl);
    }

    public async Task<BlobResponseDto> UploadAsync(IFormFile blob, string bucketName)
    {
        BlobResponseDto response = new();

        string objectName = blob.FileName;

        bool isBucketExisted = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
            .WithBucket(bucketName));

        if (!isBucketExisted)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs()
                .WithBucket(bucketName));
        }

        using (Stream stream = blob.OpenReadStream())
        {
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(blob.ContentType));
        }

        Uri blobUri = await GetUriByFileNameAsync(bucketName, objectName);

        response.Status = $"File {blob.FileName} Uploaded Successfully";
        response.Error = false;
        response.Blob.Uri = blobUri.ToString();
        response.Blob.Name = blob.FileName;
        response.Blob.ContentType = blob.ContentType;
        response.Blob.Size = blob.Length;

        return response;
    }


    public async Task<BlobDto?> DownloadAsync(string bucketName, string objectName)
    {
        try
        {
            var blobDto = new BlobDto();

            ObjectStat objectStat = await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream((stream) =>
                {
                    blobDto.Content = stream;
                }));

            blobDto.Name = objectName;
            blobDto.ContentType = objectStat.ContentType;

            return blobDto;
        }
        catch (MinioException me)
        {
            _logger.Error(me, "END: DownloadAsync - Error: {ErrorMessage}", me.Message);
            return null;
        }
    }

    public async Task<BlobResponseDto> DeleteAsync(string bucketName, string objectName)
    {
        try
        {
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName));

            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName));

            return new BlobResponseDto { Error = false, Status = $"File: {objectName} has been successfully deleted" };
        }
        catch (MinioException me)
        {
            _logger.Error(me, "END: DeleteAsync - Error: {ErrorMessage}", me.Message);
            return new BlobResponseDto { Error = true, Status = $"File: Failed to delete {objectName} file" };
        }
    }
}
