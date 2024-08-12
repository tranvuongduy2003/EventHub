using Microsoft.AspNetCore.Http;

namespace EventHub.Shared.DTOs.File;

public class UploadFileDto
{
    public IFormFile File { get; set; }
}