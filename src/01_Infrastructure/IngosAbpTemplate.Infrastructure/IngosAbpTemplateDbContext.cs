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
using IngosAbpTemplate.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

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

            //  Include Abp modules to your migration db context
            //
            modelBuilder.ConfigureAuditLogging();
            modelBuilder.ConfigureBackgroundJobs();
            modelBuilder.ConfigureTenantManagement();

            modelBuilder.ConfigureAbpEntities();
            modelBuilder.ConfigureIngosAbpTemplate();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var configuration = DbContextMigrationsFactory.BuildConfiguration();

        //    var connectionString = configuration.GetConnectionString("Default");

        //    optionsBuilder
        //        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        //        .UseSnakeCaseNamingConvention();
        //}
    }
}