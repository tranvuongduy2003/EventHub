using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Auth.GetUserProfile;

/// <summary>
/// Represents a query to retrieve a user's profile information using an access token.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of user profile details associated with the provided access token.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user whose profile is being retrieved.
/// </summary>
public record GetUserProfileQuery() : IQuery<UserDto>;
