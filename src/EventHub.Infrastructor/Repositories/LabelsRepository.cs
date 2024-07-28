using EventHub.Domain.Contracts;
using EventHub.Domain.Entities;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Data;

namespace EventHub.Infrastructor.Repositories;

public class LabelsRepository : RepositoryBase<Label>, ILabelsRepository
{
    public LabelsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}