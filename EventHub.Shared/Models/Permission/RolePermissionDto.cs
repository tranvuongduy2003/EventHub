namespace EventHub.Shared.Models.Permission;

public class RolePermissionModel
{
    public List<string> FunctionIds { get; set; } = new();

    public List<string> FunctionNames { get; set; } = new();

    public string RoleId { get; set; }

    public string RoleName { get; set; }
}