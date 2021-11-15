using IngosAbpTemplate.Application.Contracts;
using IngosAbpTemplate.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace IngosAbpTemplate.Application
{
    /// <summary>
    ///     Application Module
    /// </summary>
    [DependsOn(
        typeof(IngosAbpTemplateDomainModule),
        typeof(IngosAbpTemplateApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationModule)
    )]
    public class IngosAbpTemplateApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<IngosAbpTemplateApplicationModule>(); });
        }
    }
}