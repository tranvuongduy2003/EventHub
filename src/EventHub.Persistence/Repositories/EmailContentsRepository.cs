using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Repositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class EmailContentsRepository : RepositoryBase<EmailContent>, IEmailContentsRepository
{
    public EmailContentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}