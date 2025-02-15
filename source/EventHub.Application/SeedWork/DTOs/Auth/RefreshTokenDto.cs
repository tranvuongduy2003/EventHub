﻿using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Application.SeedWork.DTOs.Auth;

public class RefreshTokenDto
{
    [SwaggerSchema("Refresh token received after login")]
    public string RefreshToken { get; set; }
}
