using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using EventHub.Abstractions.Services;
using EventHub.Shared.DTOs.File;
using EventHub.Shared.Settings;
using Microsoft.AspNetCore.Http;

namespace EventHub.Infrastructure.FilesSystem;

/// <summary>
/// Provides file management services using Azure Blob Storage.
/// </summary>
/// <remarks>
/// This class handles file operations such as uploading, downloading, and deleting files in an Azure Blob Storage bucketName.
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
    /// The constructor sets up the Azure Blob Storage service client and retrieves the bucketName client for file operations.
    /// - <paramref name="azureBlobStorage"/>: Provides storage account details, including the account name, key, and bucketName name.
    /// </remarks>
    public AzureFileService(AzureBlobStorage azureBlobStorage)
    {
        _azureBlobStorage = azureBlobStorage;

        var credential = new StorageSharedKeyCredential(_azureBlobStorage.StorageAccount, _azureBlobStorage.Key);
        string blobUri =
            $"https://{_azureBlobStorage.StorageAccount}.blob.core.windows.net/{_azureBlobStorage.ContainerName}";
        var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        _filesContainer = blobServiceClient.GetBlobContainerClient(_azureBlobStorage.ContainerName);
    }

    public async Task<Uri> GetUriByFileNameAsync(string bucketName, string objectName)
    {
        return await Task.Run(() =>
        {
            BlobClient blobClient = _filesContainer.GetBlobClient($"{bucketName}/{objectName}");

            // Check if BlobContainerClient object has been authorized with Shared Key
            if (blobClient != null && blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one day
                var sasBuilder = new BlobSasBuilder(BlobContainerSasPermissions.Read, DateTimeOffset.UtcNow.AddDays(1))
                {
                    BlobContainerName = _azureBlobStorage.ContainerName,
                    BlobName = blobClient.Name,
                    Resource = "b",
                    Protocol = SasProtocol.HttpsAndHttp
                };

                Uri sasURI = blobClient.GenerateSasUri(sasBuilder);

                return sasURI;
            }

            // Client object is not authorized via Shared Key
            return null;
        });
    }

    public async Task<BlobResponseDto> UploadAsync(IFormFile blob, string bucketName)
    {
        BlobResponseDto response = new();
        BlobClient client = _filesContainer.GetBlobClient($"{bucketName}/{blob.FileName}");

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
            await using (Stream data = blob.OpenReadStream())
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

    public async Task<BlobDto?> DownloadAsync(string bucketName, string objectName)
    {
        BlobClient blobClient = _filesContainer.GetBlobClient($"{bucketName}/{objectName}");

        if (await blobClient.ExistsAsync())
        {
            Stream data = await blobClient.OpenReadAsync();

            Response<BlobDownloadResult> content = await blobClient.DownloadContentAsync();

            string contentType = content.Value.Details.ContentType;

            return new BlobDto { Content = data, Name = objectName, ContentType = contentType };
        }

        return null;
    }

    public async Task<BlobResponseDto> DeleteAsync(string bucketName, string objectName)
    {
        BlobClient blobClient = _filesContainer.GetBlobClient($"{bucketName}/{objectName}");

        await blobClient.DeleteIfExistsAsync();

        return new BlobResponseDto { Error = false, Status = $"File: {objectName} has been successfully deleted" };
    }
}
