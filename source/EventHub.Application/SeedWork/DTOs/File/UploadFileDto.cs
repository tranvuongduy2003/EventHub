using Microsoft.AspNetCore.Http;

namespace EventHub.Application.SeedWork.DTOs.File;

public class UploadFileDto
{
    public IFormFile File { get; set; }

    public string BucketName { get; set; }
}
