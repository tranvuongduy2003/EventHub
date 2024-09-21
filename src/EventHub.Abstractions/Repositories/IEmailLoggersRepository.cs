using EventHub.Abstractions.SeedWork.Repository;
using EventHub.Domain.AggregateModels.EmailLoggerAggregate;

namespace EventHub.Abstractions.Repositories;

public interface IEmailLoggersRepository : IRepositoryBase<EmailLogger>
{
}