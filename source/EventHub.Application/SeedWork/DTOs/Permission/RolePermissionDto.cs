using EventHub.Application.SeedWork.DTOs.Function;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Permission;

public class RolePermissionDto
{
    [SwaggerSchema("Unique identifier for the role permission.")]
    public Guid Id { get; set; }

    [SwaggerSchema("Name of the role permission.")]
    public string Name { get; set; }

    [SwaggerSchema("List of functions associated with the role permission.")]
    public List<FunctionDto> Functions { get; set; } = new();
}
