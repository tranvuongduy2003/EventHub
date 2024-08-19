using EventHub.Domain.AggregateModels.EmailLoggerAggregate;
using EventHub.Domain.SeedWork.Repository;

namespace EventHub.Domain.Repositories;

public interface IEmailLoggersRepository : IRepositoryBase<EmailLogger>
{
}