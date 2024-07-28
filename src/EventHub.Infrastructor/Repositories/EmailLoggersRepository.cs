using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class EmailLoggersRepository : RepositoryBase<EmailLogger>, IEmailLoggersRepository
{
    public EmailLoggersRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}