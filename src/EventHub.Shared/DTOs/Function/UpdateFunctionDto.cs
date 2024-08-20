using System.ComponentModel;

namespace EventHub.Shared.DTOs.Function;

public class UpdateFunctionDto : CreateFunctionDto
{
    [DefaultValue("a1b2c3x4y5z6a1b2c3x4y5z6")]
    [Description("Id of the function")]
    public string Id { get; set; }
}