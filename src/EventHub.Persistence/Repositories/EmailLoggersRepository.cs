using EventHub.Domain.AggregateModels.EmailLoggerAggregate;
using EventHub.Domain.Repositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class EmailLoggersRepository : RepositoryBase<EmailLogger>, IEmailLoggersRepository
{
    public EmailLoggersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}