using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class EmailContentsRepository : RepositoryBase<EmailContent>, IEmailContentsRepository
{
    public EmailContentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}