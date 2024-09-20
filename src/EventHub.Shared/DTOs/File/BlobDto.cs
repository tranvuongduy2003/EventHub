using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.File;

public class BlobDto
{
    [SwaggerSchema("URI of the blob resource")]
    [DefaultValue("https://example.com/blob/12345")]
    public string? Uri { get; set; }

    [SwaggerSchema("Name of the blob file")]
    [DefaultValue("example-file.txt")]
    public string? Name { get; set; }

    [SwaggerSchema("MIME type of the blob content")]
    [DefaultValue("text/plain")]
    public string? ContentType { get; set; }

    [SwaggerSchema("Stream representing the content of the blob")]
    // No DefaultValue for Stream since it's typically assigned at runtime
    public Stream? Content { get; set; }

    [SwaggerSchema("Size of the blob content in bytes")]
    [DefaultValue(1024)] // Example size of 1KB
    public long Size { get; set; } = 0;
}