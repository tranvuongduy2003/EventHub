﻿using System.Linq.Expressions;
using EventHub.Domain.SeedWork.Entities;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore.Query;

namespace EventHub.Domain.SeedWork.Persistence;

/// <summary>
/// Defines the contract for a repository that manages entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity managed by this repository. It must derive from <see cref="EntityBase"/>.</typeparam>
/// <remarks>
/// This interface provides basic repository operations including querying, creating, updating, and deleting entities.
/// It also includes support for tracking changes, including related properties, and asynchronous operations.
/// </remarks>
public interface IRepositoryBase<T> where T : EntityBase
{
    #region Update Methods

    /// <summary>
    /// Asynchronously updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    Task Update(T entity);

    #endregion

    #region Query Methods

    /// <summary>
    /// Asynchronously retrieves all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="trackChanges">Indicates whether changes to the entities should be tracked.</param>
    /// <returns>A task representing the asynchronous operation. The task result is an <see cref="IQueryable{T}"/> that can be used to query all entities of type <typeparamref name="T"/>.</returns>
    IQueryable<T> FindAll(bool trackChanges = false);

    /// <summary>
    /// Asynchronously retrieves all entities of type <typeparamref name="T"/> with specified related properties included.
    /// </summary>
    /// <param name="trackChanges">Indicates whether changes to the entities should be tracked.</param>
    /// <param name="includeProperties">An array of expressions specifying related properties to include in the query.</param>
    /// <returns>A task representing the asynchronous operation. The task result is an <see cref="IQueryable{T}"/> that can be used to query all entities of type <typeparamref name="T"/> including specified related properties.</returns>
    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties);

    /// <summary>
    /// Asynchronously retrieves entities of type <typeparamref name="T"/> that satisfy a specified condition.
    /// </summary>
    /// <param name="expression">An expression defining the condition to be satisfied by the entities.</param>
    /// <param name="trackChanges">Indicates whether changes to the entities should be tracked.</param>
    /// <returns>A task representing the asynchronous operation. The task result is an <see cref="IQueryable{T}"/> that can be used to query entities of type <typeparamref name="T"/> that satisfy the specified condition.</returns>
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

    /// <summary>
    /// Asynchronously retrieves entities of type <typeparamref name="T"/> that satisfy a specified condition with related properties included.
    /// </summary>
    /// <param name="expression">An expression defining the condition to be satisfied by the entities.</param>
    /// <param name="trackChanges">Indicates whether changes to the entities should be tracked.</param>
    /// <param name="includeProperties">An array of expressions specifying related properties to include in the query.</param>
    /// <returns>A task representing the asynchronous operation. The task result is an <see cref="IQueryable{T}"/> that can be used to query entities of type <typeparamref name="T"/> that satisfy the specified condition including specified related properties.</returns>
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);

    /// <summary>
    /// Performs a paginated and filtered query on the dataset with optional inclusion of related entities .
    /// </summary>
    /// <typeparam name="T">The type of the entity being queried.</typeparam>
    /// <param name="filter">The pagination and filtering criteria.</param>
    /// <param name="includeProperties">Optional navigation properties to include in the query.</param>
    /// <returns>A <see cref="Pagination{T}"/> object containing the paginated items and metadata.</returns>
    Pagination<T> PaginatedFind(PaginationFilter filter, bool trackChanges = false);

    /// <summary>
    /// Performs a paginated and filtered query on the dataset with optional inclusion of related entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity being queried.</typeparam>
    /// <param name="filter">The pagination and filtering criteria.</param>
    /// <param name="includePaths"></param>
    /// <param name="trackChanges"></param>
    /// <returns>A <see cref="Pagination{T}"/> object containing the paginated items and metadata.</returns>
    Pagination<T> PaginatedFind(PaginationFilter filter,
        Func<IQueryable<T>, IQueryable<T>> includePaths, bool trackChanges = false);

    /// <summary>
    /// Performs a paginated and filtered query on the dataset with optional inclusion of related entities that satisfy a specified condition with related properties included.
    /// </summary>
    /// <typeparam name="T">The type of the entity being queried.</typeparam>
    /// <param name="expression">An expression defining the condition to be satisfied by the entities.</param>
    /// <param name="filter">The pagination and filtering criteria.</param>
    /// <param name="includeProperties">Optional navigation properties to include in the query.</param>
    /// <returns>A <see cref="Pagination{T}"/> object containing the paginated items and metadata.</returns>
    Pagination<T> PaginatedFindByCondition(Expression<Func<T, bool>> expression, PaginationFilter filter,
        bool trackChanges = false);

    /// <summary>
    /// Performs a paginated and filtered query on the dataset with optional inclusion of related entities that satisfy a specified condition with related properties included.
    /// </summary>
    /// <typeparam name="T">The type of the entity being queried.</typeparam>
    /// /// <param name="expression">An expression defining the condition to be satisfied by the entities.</param>
    /// <param name="filter">The pagination and filtering criteria.</param>
    /// <param name="includePaths"></param>
    /// <param name="trackChanges"></param>
    /// <returns>A <see cref="Pagination{T}"/> object containing the paginated items and metadata.</returns>
    Pagination<T> PaginatedFindByCondition(Expression<Func<T, bool>> expression, PaginationFilter filter,
        Func<IQueryable<T>, IQueryable<T>> includePaths, bool trackChanges = false);

    #endregion

    #region Existence Methods

    /// <summary>
    /// Asynchronously checks whether an entity with the specified identifier exists in the repository.
    /// </summary>
    /// <param name="id">The identifier of the entity to check.</param>
    /// <returns>A task representing the asynchronous operation. The task result is <c>true</c> if the entity exists; otherwise, <c>false</c>.</returns>
    Task<bool> ExistAsync(Guid id);

    /// <summary>
    /// Asynchronously checks whether any entities satisfying a specified condition exist in the repository.
    /// </summary>
    /// <param name="expression">An expression defining the condition to check.</param>
    /// <returns>A task representing the asynchronous operation. The task result is <c>true</c> if any entities satisfying the condition exist; otherwise, <c>false</c>.</returns>
    Task<bool> ExistAsync(Expression<Func<T, bool>> expression);

    #endregion
    
    #region Counting Methods
    
    /// <summary>
    /// Counts the number of entities in the repository.
    /// </summary>
    /// <returns>The task result is the total count of entities.</returns>
    int Count();

    /// <summary>
    /// Asynchronously counts the number of entities in the repository.
    /// </summary>
    /// <returns>A task representing the operation. The task result is the total count of entities.</returns>
    Task<int> CountAsync();

    #endregion

    #region Retrieval Methods

    /// <summary>
    /// Asynchronously retrieves an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the entity if found; otherwise, <c>null</c>.</returns>
    Task<T> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves an entity by its identifier with related properties included.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <param name="includeProperties">An array of expressions specifying related properties to include in the query.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the entity if found; otherwise, <c>null</c>.</returns>
    Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);

    #endregion

    #region Creation Methods

    /// <summary>
    /// Asynchronously creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAsync(T entity);

    /// <summary>
    /// Asynchronously creates a list of new entities.
    /// </summary>
    /// <param name="entities">The list of entities to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateListAsync(IEnumerable<T> entities);

    #endregion

    #region Deletion Methods

    /// <summary>
    /// Deletes an existing entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    Task Delete(T entity);

    /// <summary>
    /// Deletes a list of entities.
    /// </summary>
    /// <param name="entities">The list of entities to delete.</param>
    Task DeleteList(IEnumerable<T> entities);

    #endregion

    #region Soft Deletion and Restoration Methods

    /// <summary>
    /// Performs a soft delete on an entity. The entity is marked as deleted but not actually removed from the database.
    /// </summary>
    /// <param name="entity">The entity to soft delete.</param>
    Task SoftDelete(T entity);

    /// <summary>
    /// Restores a soft-deleted entity. The entity is marked as active again.
    /// </summary>
    /// <param name="entity">The entity to restore.</param>
    Task Restore(T entity);

    #endregion
}
