// -----------------------------------------------------------------------
// <copyright file= "BaseController.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021-03-27 17:05
// Modified by:
// Description:
// -----------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.PermissionManagement;

namespace IngosAbpTemplate.Infrastructure.EntityConfigurations.Volo
{
    public static class IngosAbpDbContextModelBuilderExtensions
    {
        public static void ConfigureIngosAuditLogging(this ModelBuilder builder)
        {
            builder.Entity<AuditLog>(b =>
            {
                b.ToTable($"{Consts.AbpDbTablePrefix}AuditLogs".ToSnakeCase(), Consts.DbSchema);

                b.ConfigureByIngosConvention();

                b.Property(x => x.ApplicationName).HasMaxLength(AuditLogConsts.MaxApplicationNameLength)
                    .HasColumnName(nameof(AuditLog.ApplicationName).ToSnakeCase());
                b.Property(x => x.ClientIpAddress).HasMaxLength(AuditLogConsts.MaxClientIpAddressLength)
                    .HasColumnName(nameof(AuditLog.ClientIpAddress).ToSnakeCase());
                b.Property(x => x.ClientName).HasMaxLength(AuditLogConsts.MaxClientNameLength)
                    .HasColumnName(nameof(AuditLog.ClientName).ToSnakeCase());
                b.Property(x => x.ClientId).HasMaxLength(AuditLogConsts.MaxClientIdLength)
                    .HasColumnName(nameof(AuditLog.ClientId).ToSnakeCase());
                b.Property(x => x.CorrelationId).HasMaxLength(AuditLogConsts.MaxCorrelationIdLength)
                    .HasColumnName(nameof(AuditLog.CorrelationId).ToSnakeCase());
                b.Property(x => x.BrowserInfo).HasMaxLength(AuditLogConsts.MaxBrowserInfoLength)
                    .HasColumnName(nameof(AuditLog.BrowserInfo).ToSnakeCase());
                b.Property(x => x.HttpMethod).HasMaxLength(AuditLogConsts.MaxHttpMethodLength)
                    .HasColumnName(nameof(AuditLog.HttpMethod).ToSnakeCase());
                b.Property(x => x.Url).HasMaxLength(AuditLogConsts.MaxUrlLength)
                    .HasColumnName(nameof(AuditLog.Url).ToSnakeCase());
                b.Property(x => x.HttpStatusCode).HasColumnName(nameof(AuditLog.HttpStatusCode).ToSnakeCase());

                if (builder.IsUsingOracle()) AuditLogConsts.MaxExceptionsLengthValue = 2000;

                b.Property(x => x.Exceptions).HasMaxLength(AuditLogConsts.MaxExceptionsLengthValue)
                    .HasColumnName(nameof(AuditLog.Exceptions).ToSnakeCase());

                b.Property(x => x.Comments).HasMaxLength(AuditLogConsts.MaxCommentsLength)
                    .HasColumnName(nameof(AuditLog.Comments).ToSnakeCase());
                b.Property(x => x.ExecutionDuration).HasColumnName(nameof(AuditLog.ExecutionDuration).ToSnakeCase());
                b.Property(x => x.ImpersonatorTenantId)
                    .HasColumnName(nameof(AuditLog.ImpersonatorTenantId).ToSnakeCase());
                b.Property(x => x.ImpersonatorUserId).HasColumnName(nameof(AuditLog.ImpersonatorUserId).ToSnakeCase());
                b.Property(x => x.UserId).HasColumnName(nameof(AuditLog.UserId).ToSnakeCase());
                b.Property(x => x.UserName).HasMaxLength(AuditLogConsts.MaxUserNameLength)
                    .HasColumnName(nameof(AuditLog.UserName).ToSnakeCase());
                b.Property(x => x.TenantId).HasColumnName(nameof(AuditLog.TenantId).ToSnakeCase());

                b.HasMany(a => a.Actions).WithOne().HasForeignKey(x => x.AuditLogId).IsRequired();
                b.HasMany(a => a.EntityChanges).WithOne().HasForeignKey(x => x.AuditLogId).IsRequired();

                b.HasIndex(x => new {x.TenantId, x.ExecutionTime});
                b.HasIndex(x => new {x.TenantId, x.UserId, x.ExecutionTime});
            });

            builder.Entity<AuditLogAction>(b =>
            {
                b.ToTable($"{Consts.AbpDbTablePrefix}AuditLogActions".ToSnakeCase(), Consts.DbSchema);

                b.ConfigureByIngosConvention();

                b.Property(x => x.AuditLogId).HasColumnName(nameof(AuditLogAction.AuditLogId).ToSnakeCase());
                b.Property(x => x.ServiceName).HasMaxLength(AuditLogActionConsts.MaxServiceNameLength)
                    .HasColumnName(nameof(AuditLogAction.ServiceName).ToSnakeCase());
                b.Property(x => x.MethodName).HasMaxLength(AuditLogActionConsts.MaxMethodNameLength)
                    .HasColumnName(nameof(AuditLogAction.MethodName).ToSnakeCase());
                b.Property(x => x.Parameters).HasMaxLength(AuditLogActionConsts.MaxParametersLength)
                    .HasColumnName(nameof(AuditLogAction.Parameters).ToSnakeCase());
                b.Property(x => x.ExecutionTime).HasColumnName(nameof(AuditLogAction.ExecutionTime).ToSnakeCase());
                b.Property(x => x.ExecutionDuration)
                    .HasColumnName(nameof(AuditLogAction.ExecutionDuration).ToSnakeCase());

                b.HasIndex(x => new {x.AuditLogId});
                b.HasIndex(x => new {x.TenantId, x.ServiceName, x.MethodName, x.ExecutionTime});
            });

            builder.Entity<EntityChange>(b =>
            {
                b.ToTable($"{Consts.AbpDbTablePrefix}EntityChanges".ToSnakeCase(), Consts.DbSchema);

                b.ConfigureByIngosConvention();

                b.Property(x => x.EntityTypeFullName).HasMaxLength(EntityChangeConsts.MaxEntityTypeFullNameLength)
                    .IsRequired().HasColumnName(nameof(EntityChange.EntityTypeFullName).ToSnakeCase());
                b.Property(x => x.EntityId).HasMaxLength(EntityChangeConsts.MaxEntityIdLength).IsRequired()
                    .HasColumnName(nameof(EntityChange.EntityId).ToSnakeCase());
                b.Property(x => x.AuditLogId).IsRequired().HasColumnName(nameof(EntityChange.AuditLogId).ToSnakeCase());
                b.Property(x => x.ChangeTime).IsRequired().HasColumnName(nameof(EntityChange.ChangeTime).ToSnakeCase());
                b.Property(x => x.ChangeType).IsRequired().HasColumnName(nameof(EntityChange.ChangeType).ToSnakeCase());
                b.Property(x => x.TenantId).HasColumnName(nameof(EntityChange.TenantId));

                b.HasMany(a => a.PropertyChanges).WithOne().HasForeignKey(x => x.EntityChangeId);

                b.HasIndex(x => new {x.AuditLogId});
                b.HasIndex(x => new {x.TenantId, x.EntityTypeFullName, x.EntityId});
            });

            builder.Entity<EntityPropertyChange>(b =>
            {
                b.ToTable($"{Consts.AbpDbTablePrefix}EntityPropertyChanges".ToSnakeCase(), Consts.DbSchema);

                b.ConfigureByIngosConvention();

                b.Property(x => x.NewValue).HasMaxLength(EntityPropertyChangeConsts.MaxNewValueLength)
                    .HasColumnName(nameof(EntityPropertyChange.NewValue).ToSnakeCase());
                b.Property(x => x.PropertyName).HasMaxLength(EntityPropertyChangeConsts.MaxPropertyNameLength)
                    .IsRequired().HasColumnName(nameof(EntityPropertyChange.PropertyName).ToSnakeCase());
                b.Property(x => x.PropertyTypeFullName)
                    .HasMaxLength(EntityPropertyChangeConsts.MaxPropertyTypeFullNameLength).IsRequired()
                    .HasColumnName(nameof(EntityPropertyChange.PropertyTypeFullName).ToSnakeCase());
                b.Property(x => x.OriginalValue).HasMaxLength(EntityPropertyChangeConsts.MaxOriginalValueLength)
                    .HasColumnName(nameof(EntityPropertyChange.OriginalValue).ToSnakeCase());

                b.HasIndex(x => new {x.EntityChangeId});
            });
        }

        public static void ConfigureIngosBackgroundJobs(this ModelBuilder builder)
        {
            builder.Entity<BackgroundJobRecord>(b =>
            {
                b.ToTable($"{Consts.AbpDbTablePrefix}BackgroundJobs".ToSnakeCase(), Consts.DbSchema);

                b.ConfigureByIngosConvention();

                b.Property(x => x.JobName).IsRequired().HasMaxLength(BackgroundJobRecordConsts.MaxJobNameLength)
                    .HasColumnName(nameof(BackgroundJobRecord.JobName).ToSnakeCase());
                b.Property(x => x.JobArgs).IsRequired().HasMaxLength(BackgroundJobRecordConsts.MaxJobArgsLength)
                    .HasColumnName(nameof(BackgroundJobRecord.JobArgs).ToSnakeCase());
                b.Property(x => x.TryCount).HasDefaultValue(0)
                    .HasColumnName(nameof(BackgroundJobRecord.TryCount).ToSnakeCase());
                b.Property(x => x.NextTryTime).HasColumnName(nameof(BackgroundJobRecord.NextTryTime).ToSnakeCase());
                b.Property(x => x.LastTryTime).HasColumnName(nameof(BackgroundJobRecord.LastTryTime).ToSnakeCase());
                b.Property(x => x.IsAbandoned).HasDefaultValue(false)
                    .HasColumnName(nameof(BackgroundJobRecord.IsAbandoned).ToSnakeCase());
                b.Property(x => x.Priority).HasDefaultValue(BackgroundJobPriority.Normal)
                    .HasColumnName(nameof(BackgroundJobRecord.Priority).ToSnakeCase());

                b.HasIndex(x => new {x.IsAbandoned, x.NextTryTime});
            });
        }

        public static void ConfigureIngosPermissionManagement(this ModelBuilder builder)
        {
            builder.Entity<PermissionGrant>(b =>
            {
                b.ToTable($"{Consts.AbpDbTablePrefix}PermissionGrants".ToSnakeCase(), Consts.DbSchema);

                b.ConfigureByIngosConvention();

                b.Property(x => x.Name).HasMaxLength(PermissionGrantConsts.MaxNameLength).IsRequired()
                    .HasColumnName(nameof(PermissionGrant.Name).ToSnakeCase());
                b.Property(x => x.ProviderName).HasMaxLength(PermissionGrantConsts.MaxProviderNameLength).IsRequired()
                    .HasColumnName(nameof(PermissionGrant.ProviderName).ToSnakeCase());
                b.Property(x => x.ProviderKey).HasMaxLength(PermissionGrantConsts.MaxProviderKeyLength).IsRequired()
                    .HasColumnName(nameof(PermissionGrant.ProviderKey).ToSnakeCase());

                b.HasIndex(x => new {x.Name, x.ProviderName, x.ProviderKey});
            });
        }
    }
}