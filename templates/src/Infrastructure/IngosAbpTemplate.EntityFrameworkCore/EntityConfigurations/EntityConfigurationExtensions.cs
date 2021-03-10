//-----------------------------------------------------------------------
// <copyright file= "EntityConfigurationExtensions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/3/7 14:44:35
// Modified by:
// Description: Entity to table configuration
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;

namespace IngosAbpTemplate.EntityFrameworkCore.EntityConfigurations
{
    public static class EntityConfigurationExtensions
    {
        /// <summary>
        ///     Configure Abp framework own tables/entities
        /// </summary>
        public static void ConfigureAbpEntities(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Audit logging module
            //
            //AbpAuditLoggingDbProperties.DbTablePrefix = $"{Consts.AbpDbTablePrefix}_";
            builder.ConfigureAuditLogging();

            //builder.Entity<AuditLogAction>().ToTable($"{Consts.AbpDbTablePrefix}_Audit_Log_Actions".ToLower());
            //builder.Entity<EntityPropertyChange>()
            //    .ToTable($"{Consts.AbpDbTablePrefix}_Entity_Property_Changes".ToLower());
            //builder.Entity<EntityChange>().ToTable($"{Consts.AbpDbTablePrefix}_Entity_Changes".ToLower());
            //builder.Entity<AuditLog>().ToTable($"{Consts.AbpDbTablePrefix}_Audit_Logs".ToLower());

            // Background job module
            //
            //BackgroundJobsDbProperties.DbTablePrefix = $"{Consts.AbpDbTablePrefix}_";
            builder.ConfigureBackgroundJobs();

            //builder.Entity<BackgroundJobRecord>()
            //    .ToTable($"{Consts.AbpDbTablePrefix}_Background_Jobs".ToLower());
        }

        /// <summary>
        ///     Configure project own tables/entities
        /// </summary>
        public static void ConfigureIngosAbpTemplate(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(IngosAbpTemplateConsts.DbTablePrefix + "YourEntities", IngosAbpTemplateConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}