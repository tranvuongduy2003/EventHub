using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class EmailAttachmentsRepository : RepositoryBase<EmailAttachment>, IEmailAttachmentsRepository
{
    public EmailAttachmentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}