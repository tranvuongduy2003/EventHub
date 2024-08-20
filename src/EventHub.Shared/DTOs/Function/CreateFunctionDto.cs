using System.ComponentModel;

namespace EventHub.Shared.DTOs.Function;

public class CreateFunctionDto
{
    [DefaultValue("PAYMENT")]
    [Description("Name of the function")]
    public string Name { get; set; }

    [DefaultValue("/general/payment")]
    [Description("URL of the function")]
    public string Url { get; set; }

    [DefaultValue(1)]
    [Description("Sorting order of the function")]
    public int SortOrder { get; set; }

    [DefaultValue("GENERAL")]
    [Description("Parent function id of the function")]
    public string? ParentId { get; set; }
}