using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;

namespace EventHub.Application.Queries.User.GetUserById;

/// <summary>
/// Represents a query to retrieve a user by their unique identifier.
/// </summary>
/// <remarks>
/// This query is used to fetch a user's details using their unique identifier.
/// </remarks>
/// <summary>
/// Gets the unique identifier of the user to be retrieved.
/// </summary>
/// <param name="UserId">
/// A <see cref="Guid"/> representing the unique identifier of the user.
/// </param>
public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;