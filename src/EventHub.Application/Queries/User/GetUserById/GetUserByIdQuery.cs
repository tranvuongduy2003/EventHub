using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;

namespace EventHub.Application.Queries.User.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;