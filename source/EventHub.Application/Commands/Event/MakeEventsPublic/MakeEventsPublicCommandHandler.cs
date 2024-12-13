using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Event.MakeEventsPublic;

public class MakeEventsPublicCommandHandler : ICommandHandler<MakeEventsPublicCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public MakeEventsPublicCommandHandler(IUnitOfWork unitOfWork,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
    }

    public async Task Handle(MakeEventsPublicCommand request, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)
            ?.Value ?? "";

        IQueryable<Domain.Aggregates.EventAggregate.Event> events = _unitOfWork.CachedEvents
            .FindByCondition(x => x.AuthorId.ToString() == userId && x.IsPrivate)
            .Join(
                request.Events,
                _event => _event.Id,
                _id => _id, (_event, _id) => _event);

        if (events.Any())
        {
            await _unitOfWork.CachedEvents.UpdateAccessStatusAsync(events, false, cancellationToken);
            await _unitOfWork.CommitAsync();
        }
    }
}
