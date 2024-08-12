using EventHub.Domain.AggregateModels.EmailLoggerAggregate;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.SeedWork.Repository;

namespace EventHub.Infrastructor.Repositories;

public class EmailLoggersRepository : RepositoryBase<EmailLogger>, IEmailLoggersRepository
{
    public EmailLoggersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}