using IngosAbpTemplate.Domain;
using IngosAbpTemplate.EntityFrameworkCore.EntityConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace IngosAbpTemplate.EntityFrameworkCore
{
    [DependsOn(
        typeof(IngosAbpTemplateDomainModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
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
                /* The main point to change your DBMS.
                 * See also IngosAbpTemplateMigrationsDbContextFactory for EF Core tooling. */
                options.UseMySQL();
            });
        }
    }
}