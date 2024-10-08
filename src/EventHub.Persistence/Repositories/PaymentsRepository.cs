﻿using EventHub.Abstractions.Repositories;
using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Persistence.Data;
using EventHub.Persistence.SeedWork.Repository;

namespace EventHub.Persistence.Repositories;

public class PaymentsRepository : RepositoryBase<Payment>, IPaymentsRepository
{
    public PaymentsRepository(ApplicationDbContext context) : base(
        context)
    {
    }
}