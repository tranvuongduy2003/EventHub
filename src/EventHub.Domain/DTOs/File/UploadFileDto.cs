using Microsoft.AspNetCore.Http;

namespace EventHub.Domain.DTOs.File;

public class UploadFileDto
{
    public IFormFile File { get; set; }
}