namespace EventHub.Shared.DTOs.Permission;

public class CreatePermissionDto
{
    public string FunctionId { get; set; }

    public string RoleId { get; set; }

    public string CommandId { get; set; }
}