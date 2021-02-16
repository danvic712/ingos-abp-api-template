//-----------------------------------------------------------------------
// <copyright file= "IngosAbpTemplateInfrastructureModule.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/12 21:38:02
// Modified by:
// Description:
//-----------------------------------------------------------------------

using IngosAbpTemplate.Domain;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace IngosAbpTemplate.Infrastructure
{
    [DependsOn(typeof(IngosAbpTemplateDomainModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule)
    )]
    public class IngosAbpTemplateInfrastructureModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IngosAbpTemplateDbContext>(options =>
            {
                // includeAllEntities: false, just generate default repositories only for aggregate roots
                options.AddDefaultRepositories();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                // The main point to change your DBMS.
                options.UseMySQL();
            });
        }
    }
}