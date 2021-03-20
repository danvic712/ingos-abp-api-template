using IngosAbpTemplate.Domain;
using IngosAbpTemplate.EntityFrameworkCore.EntityConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
#if MsSQL
using Volo.Abp.EntityFrameworkCore.SqlServer;
#elif PgSQL
using Volo.Abp.EntityFrameworkCore.PostgreSql;
#else
using Volo.Abp.EntityFrameworkCore.MySQL;
#endif
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace IngosAbpTemplate.EntityFrameworkCore
{
    [DependsOn(
        typeof(IngosAbpTemplateDomainModule),
#if MsSQL
        typeof(AbpEntityFrameworkCoreSqlServerModule),
#elif PgSQL
        typeof(AbpEntityFrameworkCorePostgreSqlModule),
#else
        typeof(AbpEntityFrameworkCoreMySQLModule),
#endif
        typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule)
    )]
    public class IngosAbpTemplateEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            EntityExtraPropertyExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IngosAbpTemplateDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
#if MsSQL
                options.UseSqlServer();
#elif PgSQL
                options.UseNpgsql();
#else
                options.UseMySQL();
#endif
            });
        }
    }
}