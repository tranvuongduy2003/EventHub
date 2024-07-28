﻿using Microsoft.EntityFrameworkCore.Storage;

namespace EventHub.Domain.Common.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    void Dispose();

    Task<int> CommitAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task EndTransactionAsync();

    Task RollbackTransactionAsync();
}