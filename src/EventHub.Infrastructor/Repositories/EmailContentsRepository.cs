using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class EmailContentsRepository : RepositoryBase<EmailContent>, IEmailContentsRepository
{
    public EmailContentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}