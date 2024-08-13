﻿using Microsoft.AspNetCore.Http;

namespace EventHub.Shared.DTOs.Category;

public class CreateCategoryDto
{
    public string Name { get; set; }

    // Image name
    public IFormFile IconImage { get; set; }

    public string Color { get; set; }
}