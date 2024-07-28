using EventHub.Domain.Models.File;
using Microsoft.AspNetCore.Http;

namespace EventHub.Domain.Interfaces;

public interface IBlobService
{
    Task<string> GetUriByFileNameAsync(string container, string fileName,
        string? storedPolicyName = null);

    Task<List<BlobModel>> ListAsync();

    Task<BlobResponseModel> UploadAsync(IFormFile blob, string container);

    Task<BlobModel?> DownloadAsync(string container, string filename);

    Task<BlobResponseModel> DeleteAsync(string container, string filename);
}