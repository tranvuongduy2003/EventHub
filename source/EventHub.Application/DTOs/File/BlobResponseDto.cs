using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.DTOs.File;

public class BlobResponseDto
{
    public BlobResponseDto()
    {
        Blob = new BlobDto();
    }

    [SwaggerSchema("Status message indicating the result of the blob operation")]
    public string? Status { get; set; } = "Success";

    [SwaggerSchema("Indicates whether an error occurred during the operation")]
    public bool Error { get; set; } = false;

    [SwaggerSchema("Blob data associated with the response")]
    public BlobDto Blob { get; set; }
}
