using Microsoft.Extensions.DependencyInjection;
using SD.Application.Services;
using SD.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SD.Application.Extensions
{
    public static class ServiceBuilderExtension
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.Scan(scan =>
            scan.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(c => c.WithAttribute<MapServiceDependencyAttribute>())
                .AsSelf()
                .WithScopedLifetime()
            );

            /* Adding ApplicationCacheService */
            services.AddTransient<IApplicationCacheService, ApplicationCacheService>();
            
        }
    }
}
