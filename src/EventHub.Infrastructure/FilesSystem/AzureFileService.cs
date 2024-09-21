using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using EventHub.Abstractions;
using EventHub.Shared.DTOs.File;
using EventHub.Shared.Settings;
using Microsoft.AspNetCore.Http;

namespace EventHub.Infrastructure.FilesSystem;

/// <summary>
/// Provides file management services using Azure Blob Storage.
/// </summary>
/// <remarks>
/// This class handles file operations such as uploading, downloading, and deleting files in an Azure Blob Storage container.
/// It uses Azure's `BlobServiceClient` and `BlobContainerClient` to interact with the Azure Blob Storage service.
/// </remarks>
public class AzureFileService : IFileService
{
    private readonly AzureBlobStorage _azureBlobStorage;
    private readonly BlobContainerClient _filesContainer;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureFileService"/> class.
    /// </summary>
    /// <param name="azureBlobStorage">An instance of <see cref="AzureBlobStorage"/> containing configuration for Azure Blob Storage.</param>
    /// <remarks>
    /// The constructor sets up the Azure Blob Storage service client and retrieves the container client for file operations.
    /// - <paramref name="azureBlobStorage"/>: Provides storage account details, including the account name, key, and container name.
    /// </remarks>
    public AzureFileService(AzureBlobStorage azureBlobStorage)
    {
        _azureBlobStorage = azureBlobStorage;

        var credential = new StorageSharedKeyCredential(_azureBlobStorage.StorageAccount, _azureBlobStorage.Key);
        var blobUri =
            $"https://{_azureBlobStorage.StorageAccount}.blob.core.windows.net/{_azureBlobStorage.ContainerName}";
        var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        _filesContainer = blobServiceClient.GetBlobContainerClient(_azureBlobStorage.ContainerName);
    }

    public async Task<string> GetUriByFileNameAsync(string container, string fileName,
        string? storedPolicyName = null)
    {
        var blobClient = _filesContainer.GetBlobClient($"{container}/{fileName}");

        // Check if BlobContainerClient object has been authorized with Shared Key
        if (blobClient != null && blobClient.CanGenerateSasUri)
        {
            // Create a SAS token that's valid for one day
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _azureBlobStorage.ContainerName,
                BlobName = blobClient.Name,
                Resource = "b"
            };

            if (storedPolicyName == null)
            {
                sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(1);
                sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
                sasBuilder.Protocol.HasFlag(SasProtocol.HttpsAndHttp);
            }
            else
            {
                sasBuilder.Identifier = storedPolicyName;
            }

            var sasURI = blobClient.GenerateSasUri(sasBuilder);

            return sasURI.ToString();
        }

        // Client object is not authorized via Shared Key
        return null;
    }

    public async Task<List<BlobDto>> ListAsync()
    {
        List<BlobDto> files = new();

        await foreach (var file in _filesContainer.GetBlobsAsync())
        {
            var uri = _filesContainer.Uri.ToString();
            var name = file.Name;
            var fullUri = $"{uri}/{name}";

            files.Add(new BlobDto
            {
                Uri = fullUri,
                Name = name,
                ContentType = file.Properties.ContentType
            });
        }

        return files;
    }

    public async Task<BlobResponseDto> UploadAsync(IFormFile blob, string container)
    {
        BlobResponseDto response = new();
        var client = _filesContainer.GetBlobClient($"{container}/{blob.FileName}");

        if (await client.ExistsAsync())
        {
            response.Status = $"File {blob.FileName} Uploaded Successfully";
            response.Error = false;
            response.Blob.Uri = client.Uri.ToString();
            response.Blob.Name = blob.FileName;
            response.Blob.ContentType = blob.ContentType;
            response.Blob.Size = blob.Length;
        }
        else
        {
            await using (var data = blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            response.Status = $"File {blob.FileName} Uploaded Successfully";
            response.Error = false;
            response.Blob.Uri = client.Uri.ToString();
            response.Blob.Name = blob.FileName;
            response.Blob.ContentType = blob.ContentType;
            response.Blob.Size = blob.Length;
        }

        return response;
    }

    public async Task<BlobDto?> DownloadAsync(string container, string filename)
    {
        var file = _filesContainer.GetBlobClient($"{container}/{filename}");

        if (await file.ExistsAsync())
        {
            var data = await file.OpenReadAsync();
            var blobContent = data;

            var content = await file.DownloadContentAsync();

            var name = filename;
            var contentType = content.Value.Details.ContentType;

            return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
        }

        return null;
    }

    public async Task<BlobResponseDto> DeleteAsync(string container, string filename)
    {
        var file = _filesContainer.GetBlobClient($"{container}/{filename}");

        await file.DeleteIfExistsAsync();

        return new BlobResponseDto { Error = false, Status = $"File: {filename} has been successfully deleted" };
    }
}