using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.File;

public class BlobResponseDto
{
    public BlobResponseDto()
    {
        Blob = new BlobDto();
    }

    [SwaggerSchema("Status message indicating the result of the blob operation")]
    [DefaultValue("Success")]
    public string? Status { get; set; } = "Success";

    [SwaggerSchema("Indicates whether an error occurred during the operation")]
    [DefaultValue(false)]
    public bool Error { get; set; } = false;

    [SwaggerSchema("Blob data associated with the response")]
    public BlobDto Blob { get; set; }
}