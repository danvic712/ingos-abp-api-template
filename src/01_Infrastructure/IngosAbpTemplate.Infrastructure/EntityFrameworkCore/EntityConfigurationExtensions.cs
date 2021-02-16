//-----------------------------------------------------------------------
// <copyright file= "EntityConfigurationExtensions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/12 21:51:34
// Modified by:
// Description: Entity to table configuration
//-----------------------------------------------------------------------

using IngosAbpTemplate.Domain.AggregateModels.BookAggregate;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.TenantManagement;

namespace IngosAbpTemplate.Infrastructure.EntityFrameworkCore
{
    public static class EntityConfigurationExtensions
    {
        /// <summary>
        ///     Configure Abp framework own tables/entities
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureAbpEntities(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Audit logging module
            //
            builder.Entity<AuditLogAction>().ToTable($"{Consts.AbpDbTablePrefix}_Audit_Log_Actions".ToLower());
            builder.Entity<EntityPropertyChange>()
                .ToTable($"{Consts.AbpDbTablePrefix}_Entity_Property_Changes".ToLower());
            builder.Entity<EntityChange>().ToTable($"{Consts.AbpDbTablePrefix}_Entity_Changes".ToLower());
            builder.Entity<AuditLog>().ToTable($"{Consts.AbpDbTablePrefix}_Audit_Logs".ToLower());

            // Background job module
            builder.Entity<BackgroundJobRecord>()
                .ToTable($"{Consts.AbpDbTablePrefix}_Background_Job_Records".ToLower());

            // Tenant management module
            //
            builder.Entity<Tenant>().ToTable($"{Consts.AbpDbTablePrefix}_Tenants".ToLower());
            builder.Entity<TenantConnectionString>()
                .ToTable($"{Consts.AbpDbTablePrefix}_Tenant_Connection_Strings".ToLower());
        }

        /// <summary>
        ///     Configure your own tables/entities
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureIngosAbpTemplate(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(BookStoreConsts.DbTablePrefix + "YourEntities", BookStoreConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});

            builder.Entity<Book>(b =>
            {
                b.ToTable($"{Consts.DbTablePrefix}_Books".ToLower(),
                    Consts.DbSchema);
                b.ConfigureByConvention(); // auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            });

            //builder.Entity<BookType>(builder =>
            //{
            //    builder.ToTable(IngosAbpTemplateConsts.DbTablePrefix + "BookType",
            //        IngosAbpTemplateConsts.DbSchema);
            //    builder.ConfigureByConvention();

            //    builder.Property(x => x.Name)
            //        .IsRequired()
            //        .HasMaxLength(128);
            //});
        }
    }
}