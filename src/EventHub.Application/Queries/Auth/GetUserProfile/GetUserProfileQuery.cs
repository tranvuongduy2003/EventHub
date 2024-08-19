using EventHub.Shared.Models.User;
using MediatR;

namespace EventHub.Application.Queries.Auth.GetUserProfile;

public record GetUserProfileQuery(string AccessToken) : IRequest<UserModel>;