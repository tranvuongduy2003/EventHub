namespace EventHub.Shared.DTOs.Permission;

public class UpdatePermissionDto
{
    public List<CreatePermissionDto> Permissions { get; set; } = new();
}