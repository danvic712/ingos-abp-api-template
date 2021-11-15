using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IngosAbpTemplate.API.Infrastructure;
using IngosAbpTemplate.Application;
using IngosAbpTemplate.Application.Contracts;
using IngosAbpTemplate.Domain;
using IngosAbpTemplate.Domain.Shared;
using IngosAbpTemplate.Domain.Shared.Localization;
using IngosAbpTemplate.Infrastructure;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace IngosAbpTemplate.API
{
    /// <summary>
    ///     Api module definition file
    /// </summary>
    [DependsOn(typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(IngosAbpTemplateApplicationModule),
        typeof(IngosAbpTemplateInfrastructureModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
    )]
    public class IngosAbpTemplateApiModule : AbpModule
    {
        private const string CorsPolicyName = "IngosAbpTemplate";

        #region Services

        /// <summary>
        ///     Pre configure before inject services into service collection
        /// </summary>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpAspNetCoreMvcOptions>(options =>
            {
                // Set dynamic api router with api version info
                options.ConventionalControllers.Create(typeof(IngosAbpTemplateApplicationModule).Assembly,
                    opts => { opts.RootPath = "v{version:apiVersion}"; });

                // Specify version info for framework built-in api
                options.ConventionalControllers.Create(typeof(AbpPermissionManagementHttpApiModule).Assembly,
                    opts => { opts.ApiVersions.Add(new ApiVersion(1, 0)); });
            });
        }

        /// <summary>
        ///     Configure application services
        /// </summary>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            context.Services.AddHttpClient();

            context.Services.AddDaprClient();

            ConfigureHealthChecks(context);
            ConfigureAntiForgeryTokens();
            ConfigureAuditing(configuration);
            ConfigureConventionalControllers(context);
            ConfigureLocalization();
            ConfigureVirtualFileSystem(context);
            ConfigureCache(context, configuration, hostingEnvironment);
            ConfigureCors(context, configuration);
            ConfigureSwagger(context);
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseAbpRequestLocalization();

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(CorsPolicyName);
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHealthChecks("/health");

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "IngosAbpTemplate API";

                // Display latest api version by default
                //
                var provider = context.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                var apiVersionList = provider.ApiVersionDescriptions
                    .Select(i => $"v{i.ApiVersion.MajorVersion}")
                    .Distinct().Reverse();
                foreach (var apiVersion in apiVersionList)
                    options.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json",
                        $"IngosAbpTemplate API {apiVersion?.ToUpperInvariant()}");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
        }

        #endregion Services

        #region Methods

        private static void ConfigureHealthChecks(ServiceConfigurationContext context)
        {
            context.Services.AddHealthChecks()
                .AddDbContextCheck<IngosAbpTemplateDbContext>();
        }

        private void ConfigureAntiForgeryTokens()
        {
            Configure<AbpAntiForgeryOptions>(options =>
            {
                options.TokenCookie.Expiration = TimeSpan.FromDays(365);
                options.AutoValidateIgnoredHttpMethods.Add("POST");
            });
        }

        private void ConfigureAuditing(IConfiguration configuration)
        {
            var applicationName = configuration["App:ApplicationName"];

            Configure<AbpAuditingOptions>(options =>
            {
                options.ApplicationName = applicationName; // Set the application name
                options.EntityHistorySelectors.AddAllEntities(); // Default saving all changes of entities
            });
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<IngosAbpTemplateDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $@"..{Path.DirectorySeparatorChar}/IngosAbpTemplate.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IngosAbpTemplateDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $@"..{Path.DirectorySeparatorChar}/IngosAbpTemplate.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IngosAbpTemplateApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $@"..{Path.DirectorySeparatorChar}/IngosAbpTemplate.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<IngosAbpTemplateApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $@"..{Path.DirectorySeparatorChar}/IngosAbpTemplate.Application"));
                });
        }

        private void ConfigureConventionalControllers(ServiceConfigurationContext context)
        {
            Configure<AbpAspNetCoreMvcOptions>(options => { context.Services.ExecutePreConfiguredActions(options); });

            // Use lowercase routing and lowercase query string
            context.Services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            context.Services.AddAbpApiVersioning(options =>
            {
                options.ReportApiVersions = true;

                options.AssumeDefaultVersionWhenUnspecified = true;

                options.DefaultApiVersion = new ApiVersion(1, 0);

                options.ApiVersionReader = new UrlSegmentApiVersionReader();

                var mvcOptions = context.Services.ExecutePreConfiguredActions<AbpAspNetCoreMvcOptions>();
                options.ConfigureAbp(mvcOptions);
            });

            context.Services.AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";

                option.AssumeDefaultVersionWhenUnspecified = true;
            });
        }

        private static void ConfigureSwagger(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(
                options =>
                {
                    // Get application api version info
                    var provider = context.Services.BuildServiceProvider()
                        .GetRequiredService<IApiVersionDescriptionProvider>();

                    // Generate swagger by api major version
                    foreach (var description in provider.ApiVersionDescriptions)
                        options.SwaggerDoc(description.GroupName, new OpenApiInfo
                        {
                            Contact = new OpenApiContact
                            {
                                Name = "Danvic Wang",
                                Email = "danvic.wang@outlook.com",
                                Url = new Uri("https://yuiter.com")
                            },
                            Description = "IngosAbpTemplate API",
                            Title = "IngosAbpTemplate API",
                            Version = $"v{description.ApiVersion.MajorVersion}"
                        });

                    options.DocInclusionPredicate((docName, description) =>
                    {
                        // Get api major version
                        var apiVersion = $"v{description.GetApiVersion().MajorVersion}";

                        if (!docName.Equals(apiVersion))
                            return false;

                        // Replace router parameter
                        var values = description.RelativePath
                            .Split('/')
                            .Select(v => v.Replace("v{version}", apiVersion));

                        description.RelativePath = string.Join("/", values);

                        return true;
                    });

                    // Let params use the camel naming method
                    options.DescribeAllParametersInCamelCase();

                    // Remove version parameter info input in swagger page
                    options.OperationFilter<RemoveVersionFromParameter>();

                    // Inject api and dto comments
                    //
                    var paths = new List<string>
                    {
                        @"wwwroot/api-doc/IngosAbpTemplate.API.xml",
                        @"wwwroot/api-doc/IngosAbpTemplate.Application.xml",
                        @"wwwroot/api-doc/IngosAbpTemplate.Application.Contracts.xml"
                    };
                    GetApiDocPaths(paths, Path.GetDirectoryName(AppContext.BaseDirectory))
                        .ForEach(x => options.IncludeXmlComments(x, true));
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

        private void ConfigureCache(ServiceConfigurationContext context, IConfiguration configuration,
            IHostEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
                return;

            // Get redis connection
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);

            var prefix = configuration["Redis:KeyPrefix"];

            // Configure cache options 
            Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = $"{prefix}:"; });

            // Add data protection 
            //
            context.Services
                .AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, $"{prefix}:protection-keys");
        }

        private static void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, builder =>
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

        /// <summary>
        ///     Get the api description doc path
        /// </summary>
        /// <param name="paths">The xml file path</param>
        /// <param name="basePath">The site's base running files path</param>
        /// <returns></returns>
        private static List<string> GetApiDocPaths(IEnumerable<string> paths, string basePath)
        {
            var files = from path in paths
                let xml = Path.Combine(basePath, path)
                select xml;

            return files.ToList();
        }

        #endregion Methods
    }
}