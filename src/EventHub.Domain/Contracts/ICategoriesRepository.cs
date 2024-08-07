﻿using EventHub.Domain.Common.Repository;
using EventHub.Domain.Entities;

namespace EventHub.Domain.Contracts;

public interface ICategoriesRepository : IRepositoryBase<Category>
{
}