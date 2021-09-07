using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Caching;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Modularity;

namespace AElf.WebServer
{
    [DependsOn(typeof(AbpAspNetCoreModule))]
    public class AElfWebServerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = services.GetConfiguration();
            services.AddSingleton<IDistributedCacheSerializer, Utf8JsonDistributedCacheSerializer>();
            services.AddSingleton<IDistributedCacheKeyNormalizer, DistributedCacheKeyNormalizer>();
            services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
            services.AddTransient<IApiDescriptionModelProvider, AspNetCoreApiDescriptionModelProvider>();
            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.UseApiBehavior = false;
            });
            services.AddVersionedApiExplorer();
            services.AddDistributedMemoryCache();

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = "Rural Assets Platform API", Version = "v1"});
                    options.OperationFilter<SwaggerFileUploadFilter>();
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );

            services.Configure<ConfigOptions>(configuration.GetSection("Config"));
        }
    }
}