namespace EventHub.Domain.Models.Auth;

public class SignInResponseModel
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}