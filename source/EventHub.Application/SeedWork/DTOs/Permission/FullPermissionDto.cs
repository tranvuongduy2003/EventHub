using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Permission;

public class FullPermissionDto
{
    [SwaggerSchema("Unique identifier for the permission.")]
    public string Id { get; set; }

    [SwaggerSchema("The name of the permission.")]
    public string Name { get; set; }

    [SwaggerSchema("Unique identifier of the parent permission.")]
    public string ParentId { get; set; }

    [SwaggerSchema("Indicates if the permission includes create access.")]
    public bool HasCreate { get; set; }

    [SwaggerSchema("Indicates if the permission includes update access.")]
    public bool HasUpdate { get; set; }

    [SwaggerSchema("Indicates if the permission includes delete access.")]
    public bool HasDelete { get; set; }

    [SwaggerSchema("Indicates if the permission includes view access.")]
    public bool HasView { get; set; }

    [SwaggerSchema("Indicates if the permission includes approve access.")]
    public bool HasApprove { get; set; }
}
