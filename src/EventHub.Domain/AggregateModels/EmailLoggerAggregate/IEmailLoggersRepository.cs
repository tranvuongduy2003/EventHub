using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.AggregateModels.EmailLoggerAggregate;

public interface IEmailLoggersRepository : IRepositoryBase<EmailLogger>
{
}