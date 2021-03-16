using IngosAbpTemplate.Domain.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;

namespace IngosAbpTemplate.Application.Contracts
{
    [DependsOn(
        typeof(IngosAbpTemplateDomainSharedModule),
        typeof(AbpObjectExtendingModule),
        typeof(AbpPermissionManagementApplicationContractsModule)
    )]
    public class IngosAbpTemplateApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            IngosAbpTemplateDtoExtensions.Configure();
        }
    }
}