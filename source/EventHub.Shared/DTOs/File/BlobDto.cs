using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.File;

public class BlobDto
{
    [SwaggerSchema("URI of the blob resource")]
    public string? Uri { get; set; }

    [SwaggerSchema("Name of the blob file")]
    public string? Name { get; set; }

    [SwaggerSchema("MIME type of the blob content")]
    public string? ContentType { get; set; }

    [SwaggerSchema("Stream representing the content of the blob")]
    public Stream? Content { get; set; }

    [SwaggerSchema("Size of the blob content in bytes")]
    public long Size { get; set; } = 0;
}
