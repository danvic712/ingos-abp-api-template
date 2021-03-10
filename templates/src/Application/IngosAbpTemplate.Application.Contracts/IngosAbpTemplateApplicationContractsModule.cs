using IngosAbpTemplate.Domain.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace IngosAbpTemplate.Application.Contracts
{
    [DependsOn(
        typeof(IngosAbpTemplateDomainSharedModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class IngosAbpTemplateApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            IngosAbpTemplateDtoExtensions.Configure();
        }
    }
}