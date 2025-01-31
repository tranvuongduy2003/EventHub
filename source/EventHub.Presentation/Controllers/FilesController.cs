﻿using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.File;
using EventHub.Domain.Shared.HttpResponses;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/files")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly ILogger<FilesController> _logger;
    private readonly IFileService _fileService;

    public FilesController(ILogger<FilesController> logger, IFileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
    }

    [HttpPost("upload/{bucketName}")]
    public async Task<IActionResult> PostUploadFile(string bucketName, [FromForm] IFormFile file)
    {
        _logger.LogInformation("START: PostUploadFile");

        BlobResponseDto uploadedFile = await _fileService.UploadAsync(file, bucketName);

        _logger.LogInformation("END: PostUploadFile");
        return Ok(new ApiOkResponse(uploadedFile));
    }
}
