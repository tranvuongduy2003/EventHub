using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class EmailAttachmentsRepository : RepositoryBase<EmailAttachment>, IEmailAttachmentsRepository
{
    public EmailAttachmentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}