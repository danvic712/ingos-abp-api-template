using IngosAbpTemplate.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace IngosAbpTemplate.Domain
{
    [DependsOn(
        typeof(IngosAbpTemplateDomainSharedModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpBackgroundJobsDomainModule),
        typeof(AbpEmailingModule),
        typeof(AbpPermissionManagementDomainModule)
    )]
    public class IngosAbpTemplateDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
#if DEBUG
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
        }
    }
}