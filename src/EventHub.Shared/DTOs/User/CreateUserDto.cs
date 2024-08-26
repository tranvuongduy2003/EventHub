using System.ComponentModel;
using System.Text.Json.Serialization;
using EventHub.Shared.Enums.User;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Shared.DTOs.User;

public class CreateUserDto
{
    [DefaultValue("user123@gmail.com")]
    [SwaggerSchema("Email will be registered for the account")]
    public string Email { get; set; }

    [DefaultValue("+84123456789")]
    [SwaggerSchema("PhoneNumber will be registered for the account")]
    public string PhoneNumber { get; set; }

    [DefaultValue("01/01/1999")]
    [SwaggerSchema("Date of birth of the user")]
    public DateTime? Dob { get; set; }

    [DefaultValue("User 123")]
    [SwaggerSchema("Full name will be registered for the account")]
    public string FullName { get; set; }

    [DefaultValue("User@123")]
    [SwaggerSchema("Password will be registered for the account")]
    public string Password { get; set; }

    [DefaultValue("user123")]
    [SwaggerSchema("User name will be registered for the account")]
    public string? UserName { get; set; } = string.Empty;


    [DefaultValue(nameof(EGender.MALE))]
    [SwaggerSchema("Gender of the user")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EGender? Gender { get; set; }

    [DefaultValue("My name is Alex Drysdale and I am a junior web developer for Oswald Technologies. I am an accomplished coder and programmer, and I enjoy using my skills to contribute to the exciting technological advances that happen every day at Oswald Tech. I graduated from the California Institute of Technology in 2016 with a bachelor's degree in software development. While in school, I earned the 2015 Edmund Gains Award for my exemplary academic performance and leadership skills.")]
    [SwaggerSchema("Biography of the user")]
    public string? Bio { get; set; }

    [SwaggerSchema("Avatar of the user")]
    public IFormFile? Avatar { get; set; }
}