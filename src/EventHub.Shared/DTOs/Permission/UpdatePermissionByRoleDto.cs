namespace EventHub.Shared.DTOs.Permission;

public class UpdatePermissionByRoleDto
{
    public string RoleId { get; set; }

    public string FunctionId { get; set; }

    public bool Value { get; set; }
}