using IngosAbpTemplate.Application.Contracts;
using IngosAbpTemplate.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace IngosAbpTemplate.Application
{
    [DependsOn(
        typeof(IngosAbpTemplateDomainModule),
        typeof(IngosAbpTemplateApplicationContractsModule)
    )]
    public class IngosAbpTemplateApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<IngosAbpTemplateApplicationModule>(); });
        }
    }
}