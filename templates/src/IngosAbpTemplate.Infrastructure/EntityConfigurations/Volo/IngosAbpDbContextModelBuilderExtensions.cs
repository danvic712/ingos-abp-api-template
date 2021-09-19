// -----------------------------------------------------------------------
// <copyright file= "IngosAbpDbContextModelBuilderExtensions.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021-05-20 21:16
// Modified by:
// Description:
// -----------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace IngosAbpTemplate.Infrastructure.EntityConfigurations.Volo
{
    public static class IngosAbpDbContextModelBuilderExtensions
    {
        public static void ConfigureIngosBackgroundJobs(
            this ModelBuilder builder,
            Action<BackgroundJobsModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            if (builder.IsTenantOnlyDatabase())
            {
                return;
            }

            var options = new BackgroundJobsModelBuilderConfigurationOptions(
                BackgroundJobsDbProperties.DbTablePrefix,
                BackgroundJobsDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<BackgroundJobRecord>(b =>
            {
                b.ToTable(options.TablePrefix + "_BackgroundJobs".ToSnakeCase(), options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.JobName).IsRequired().HasMaxLength(BackgroundJobRecordConsts.MaxJobNameLength)
                    .HasColumnName(nameof(BackgroundJobRecord.JobName).ToSnakeCase());
                b.Property(x => x.JobArgs).IsRequired().HasMaxLength(BackgroundJobRecordConsts.MaxJobArgsLength)
                    .HasColumnName(nameof(BackgroundJobRecord.JobArgs).ToSnakeCase());
                b.Property(x => x.TryCount).HasDefaultValue(0)
                    .HasColumnName(nameof(BackgroundJobRecord.TryCount).ToSnakeCase());
                b.Property(x => x.NextTryTime)
                    .HasColumnName(nameof(BackgroundJobRecord.NextTryTime).ToSnakeCase());
                b.Property(x => x.LastTryTime)
                    .HasColumnName(nameof(BackgroundJobRecord.LastTryTime).ToSnakeCase());
                b.Property(x => x.IsAbandoned).HasDefaultValue(false)
                    .HasColumnName(nameof(BackgroundJobRecord.IsAbandoned).ToSnakeCase());
                b.Property(x => x.Priority).HasDefaultValue(BackgroundJobPriority.Normal)
                    .HasColumnName(nameof(BackgroundJobRecord.Priority).ToSnakeCase());

                b.HasIndex(x => new { x.IsAbandoned, x.NextTryTime });
            });
        }
    }
}