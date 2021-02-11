//-----------------------------------------------------------------------
// <copyright file= "IngosAbpTemplateApplicationModule.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/11 16:32:27
// Modified by:
// Description:
//-----------------------------------------------------------------------

using IngosAbpTemplate.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace IngosAbpTemplate.Application
{
    [DependsOn(typeof(IngosAbpTemplateDomainModule),
        typeof(AbpAutoMapperModule))]
    public class IngosAbpTemplateApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Configure AutoMapper
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<IngosAbpTemplateApplicationModule>(); });
        }
    }
}