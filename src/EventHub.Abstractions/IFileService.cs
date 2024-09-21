using EventHub.Shared.DTOs.File;
using Microsoft.AspNetCore.Http;

namespace EventHub.Abstractions;

/// <summary>
/// Defines the contract for file management services, including operations for uploading, downloading, and deleting files.
/// </summary>
/// <remarks>
/// This interface provides methods for interacting with a file storage system, including obtaining file URIs, listing files, and managing file operations.
/// It is designed to work with various file storage solutions and supports operations for different containers and files.
/// </remarks>
public interface IFileService
{
    /// <summary>
    /// Retrieves the URI of a file in the specified container.
    /// </summary>
    /// <param name="container">The name of the container where the file is stored.</param>
    /// <param name="fileName">The name of the file for which to retrieve the URI.</param>
    /// <param name="storedPolicyName">An optional name of a stored access policy for the container.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the URI of the file.</returns>
    /// <remarks>
    /// If a stored policy name is provided, the URI will be generated based on the policy.
    /// </remarks>
    Task<string> GetUriByFileNameAsync(string container, string fileName, string? storedPolicyName = null);

    /// <summary>
    /// Lists all files in the storage system.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of file metadata.</returns>
    /// <remarks>
    /// The list of files returned includes metadata about each file, such as names and other relevant information.
    /// </remarks>
    Task<List<BlobDto>> ListAsync();

    /// <summary>
    /// Uploads a file to the specified container.
    /// </summary>
    /// <param name="blob">The file to upload.</param>
    /// <param name="container">The name of the container to upload the file to.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response details of the uploaded file.</returns>
    /// <remarks>
    /// The file is uploaded to the specified container. The response details include information about the uploaded file.
    /// </remarks>
    Task<BlobResponseDto> UploadAsync(IFormFile blob, string container);

    /// <summary>
    /// Downloads a file from the specified container.
    /// </summary>
    /// <param name="container">The name of the container where the file is stored.</param>
    /// <param name="filename">The name of the file to download.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the file metadata and content if available, or null if not found.</returns>
    /// <remarks>
    /// If the file is found, the metadata and content are returned. If the file does not exist, the result will be null.
    /// </remarks>
    Task<BlobDto?> DownloadAsync(string container, string filename);

    /// <summary>
    /// Deletes a file from the specified container.
    /// </summary>
    /// <param name="container">The name of the container where the file is stored.</param>
    /// <param name="filename">The name of the file to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response details of the deleted file.</returns>
    /// <remarks>
    /// The file is removed from the specified container. The response details include information about the deleted file.
    /// </remarks>
    Task<BlobResponseDto> DeleteAsync(string container, string filename);
}