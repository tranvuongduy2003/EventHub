using EventHub.Domain.SeedWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EventHub.Persistence.Interceptors;

/// <summary>
/// An EF Core interceptor that updates date tracking properties for entities implementing the IDateTracking interface.
/// </summary>
/// <remarks>
/// This class extends <see cref="SaveChangesInterceptor"/> to intercept the `SavingChangesAsync` event
/// and ensure that entities implementing the <see cref="IDateTracking"/> interface have their date
/// tracking properties updated appropriately before changes are saved to the database.
/// </remarks>
public sealed class DateTrackingInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Intercepts the `SaveChanges` operation of a `DbContext` to update date tracking properties
    /// on entities that implement the `IDateTracking` interface.
    /// </summary>
    /// <param name="eventData">
    /// The <see cref="DbContextEventData"/> that provides information about the <see cref="DbContext"/> 
    /// in which the changes are being tracked and saved. This parameter allows access to the 
    /// `DbContext` instance that is performing the save operation.
    /// </param>
    /// <param name="result">
    /// The current <see cref="InterceptionResult{TResult}"/> representing the result of the intercepted 
    /// operation. This parameter allows you to modify or inspect the outcome of the `SaveChanges` operation.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to observe while waiting for the operation 
    /// to complete. This parameter allows the operation to be cancelled if needed. The default value is 
    /// <see cref="default"/> which means no cancellation is requested.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> that represents the asynchronous operation. The result of 
    /// this task is an <see cref="InterceptionResult{TResult}"/> that indicates the result of the 
    /// `SaveChanges` operation after any additional logic in the interceptor has been applied.
    /// </returns>
    /// <remarks>
    /// This method updates the `CreatedAt` property to the current UTC date and time for entities 
    /// that are being added and updates the `UpdatedAt` property for entities that are being modified. 
    /// It only affects entities that implement the `IDateTracking` interface. The method is called 
    /// during the `SaveChanges` operation of the `DbContext` to ensure that date tracking information 
    /// is updated before the changes are saved to the database.
    /// </remarks>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext != null)
        {
            var modified = dbContext.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Modified or EntityState.Added);

            foreach (var item in modified)
            {
                if (item.Entity is not IDateTracking changedOrAddedItem) continue;
                if (item.State == EntityState.Added)
                {
                    changedOrAddedItem.CreatedAt = DateTime.UtcNow;
                    changedOrAddedItem.UpdatedAt = DateTime.UtcNow;
                }
                else
                    changedOrAddedItem.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}