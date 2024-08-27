using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;

namespace EventHub.Application.Queries.Auth.GetUserProfile;

/// <summary>
/// Represents a query to retrieve a user's profile information using an access token.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of user profile details associated with the provided access token.
/// </remarks>
public record GetUserProfileQuery(string AccessToken) : IQuery<UserDto>;