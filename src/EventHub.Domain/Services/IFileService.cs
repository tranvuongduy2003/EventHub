using EventHub.Shared.DTOs.File;
using Microsoft.AspNetCore.Http;

namespace EventHub.Domain.Services;

public interface IFileService
{
    Task<string> GetUriByFileNameAsync(string container, string fileName,
        string? storedPolicyName = null);

    Task<List<BlobDto>> ListAsync();

    Task<BlobResponseDto> UploadAsync(IFormFile blob, string container);

    Task<BlobDto?> DownloadAsync(string container, string filename);

    Task<BlobResponseDto> DeleteAsync(string container, string filename);
}