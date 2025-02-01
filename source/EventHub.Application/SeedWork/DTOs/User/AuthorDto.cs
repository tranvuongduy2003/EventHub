using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.User;

public class AuthorDto
{
    [SwaggerSchema("Unique identifier for the author.")]
    public string Id { get; set; }

    [SwaggerSchema("The email address of the author.")]
    public string Email { get; set; }

    [SwaggerSchema("The full name of the author.")]
    public string FullName { get; set; }

    [SwaggerSchema("The avatar URL or path for the author.")]
    public string? Avatar { get; set; }

    public bool IsFollowed { get; set; }
}
