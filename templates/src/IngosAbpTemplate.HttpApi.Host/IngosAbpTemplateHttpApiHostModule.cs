using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IngosAbpTemplate.Application;
using IngosAbpTemplate.Application.Contracts;
using IngosAbpTemplate.Domain;
using IngosAbpTemplate.Domain.Shared;
using IngosAbpTemplate.Domain.Shared.Localization;
using IngosAbpTemplate.EntityFrameworkCore;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace IngosAbpTemplate.HttpApi.Host
{
    [DependsOn(typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(IngosAbpTemplateApplicationModule),
        typeof(IngosAbpTemplateEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
    )]
    public class IngosAbpTemplateHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "IngosAbpTemplate";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureConventionalControllers(context.Services);
            ConfigureAuthentication(context, configuration);
            ConfigureLocalization();
            ConfigureCache(configuration);
            ConfigureVirtualFileSystem(context);
            ConfigureRedis(context, configuration, hostingEnvironment);
            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context, configuration);
        }

        private void ConfigureCache(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "IngosAbpTemplate:"; });
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<IngosAbpTemplateDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $@"..{Path.DirectorySeparatorChar}\Domain\IngosAbpTemplate.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IngosAbpTemplateDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $@"..{Path.DirectorySeparatorChar}\Domain\IngosAbpTemplate.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IngosAbpTemplateApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $@"..{Path.DirectorySeparatorChar}\Application\IngosAbpTemplate.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IngosAbpTemplateApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $@"..{Path.DirectorySeparatorChar}\Application\IngosAbpTemplate.Application"));
                });
        }

        private void ConfigureConventionalControllers(IServiceCollection services)
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(IngosAbpTemplateApplicationModule).Assembly);
            });

            // Use lowercase routing and lowercase query string
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = "IngosAbpTemplate";
                });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                    {"IngosAbpTemplate", "IngosAbpTemplate API"}
                },
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "IngosAbpTemplate API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);

                    // Let params use the camel naming method
                    options.DescribeAllParametersInCamelCase();
                });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<IngosAbpTemplateResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );

                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });
        }

        private void ConfigureRedis(
            ServiceConfigurationContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "IngosAbpTemplate-Protection-Keys");
            }
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseAbpRequestLocalization();

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "IngosAbpTemplate API");

                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
        }
    }
}