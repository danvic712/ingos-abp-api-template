//-----------------------------------------------------------------------
// <copyright file= "IngosAbpTemplateDbContext.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/12 21:30:41
// Modified by:
// Description:
//-----------------------------------------------------------------------

using IngosAbpTemplate.Domain.AggregateModels.BookAggregate;
using IngosAbpTemplate.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace IngosAbpTemplate.Infrastructure
{
    [ConnectionStringName("Default")]
    public class IngosAbpTemplateDbContext : AbpDbContext<IngosAbpTemplateDbContext>
    {
        public IngosAbpTemplateDbContext(DbContextOptions<IngosAbpTemplateDbContext> options) : base(options)
        {
        }

        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside IngosAbpTemplateEntityConfigurationExtensions.ConfigureIngosAbpTemplate
         */

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureIngosAbpTemplate();
        }
    }
}