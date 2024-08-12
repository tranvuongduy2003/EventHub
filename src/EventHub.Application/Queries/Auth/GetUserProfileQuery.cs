using EventHub.Shared.Models.User;
using MediatR;

namespace EventHub.Application.Queries.Auth;

public class GetUserProfileQuery : IRequest<UserModel>
{
    public GetUserProfileQuery(string accessToken)
        => AccessToken = accessToken;
    
    public string AccessToken { get; set; }
}