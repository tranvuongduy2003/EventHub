using EventHub.Shared.DTOs.User;
using MediatR;

namespace EventHub.Application.Queries.Auth.GetUserProfile;

public record GetUserProfileQuery(string AccessToken) : IRequest<UserDto>;