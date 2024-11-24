﻿using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.Function;

public class FunctionDto
{
    [SwaggerSchema("Id of the function")]
    public string Id { get; set; }

    [SwaggerSchema("Name of the function")]
    public string Name { get; set; }

    [SwaggerSchema("URL of the function")]
    public string Url { get; set; }

    [SwaggerSchema("Sorting order of the function")]
    public int SortOrder { get; set; }

    [SwaggerSchema("Parent function id of the function")]
    public string ParentId { get; set; }
}