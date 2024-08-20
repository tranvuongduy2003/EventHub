using System.ComponentModel;

namespace EventHub.Shared.DTOs.Function;

public class FunctionDto
{
    [Description("Id of the function")]
    public string Id { get; set; }

    [Description("Name of the function")]
    public string Name { get; set; }

    [Description("URL of the function")]
    public string Url { get; set; }

    [Description("Sorting order of the function")]
    public int SortOrder { get; set; }

    [Description("Parent function id of the function")]
    public string ParentId { get; set; }
}