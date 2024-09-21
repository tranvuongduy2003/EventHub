using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class EmailAttachmentsRepository : RepositoryBase<EmailAttachment>, IEmailAttachmentsRepository
{
    public EmailAttachmentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}