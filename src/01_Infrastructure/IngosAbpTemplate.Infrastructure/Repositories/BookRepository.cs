//-----------------------------------------------------------------------
// <copyright file= "BookRepository.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/12 23:07:40
// Modified by:
// Description:
//-----------------------------------------------------------------------

using IngosAbpTemplate.Domain.AggregateModels.BookAggregate;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace IngosAbpTemplate.Infrastructure.Repositories
{
    public class BookRepository : EfCoreRepository<IngosAbpTemplateDbContext, Book>
    {
        public BookRepository(IDbContextProvider<IngosAbpTemplateDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}