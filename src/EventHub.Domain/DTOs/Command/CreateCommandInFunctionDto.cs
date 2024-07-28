namespace EventHub.Domain.DTOs.Command;

public class CreateCommandInFunctionDto
{
    public string CommandId { get; set; }

    public string FunctionId { get; set; }
}