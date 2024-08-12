namespace EventHub.Shared.DTOs.Permission;

public class UpdatePermissionByCommandDto
{
    public string FunctionId { get; set; }

    public string CommandId { get; set; }

    public bool Value { get; set; }
}