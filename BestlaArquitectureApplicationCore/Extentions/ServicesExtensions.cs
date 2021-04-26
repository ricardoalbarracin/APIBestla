using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using BestlaArquitectureApplicationCore.Interfaces;
using Castle.DynamicProxy;
using System.Reflection;
using BestlaArquitectureApplicationCore.Entities;

namespace BestlaArquitectureApplicationCore.Extentions
{
    public static class ServicesExtensions
    {
        public static void AddProxiedScoped<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddScoped<TImplementation>();
            services.AddScoped(typeof(TInterface), serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService<TImplementation>();
                var interceptors = serviceProvider.GetServices<AsyncInterceptorBase>().ToArray();
                return proxyGenerator.CreateInterfaceProxyWithTargetInterface(typeof(TInterface), actual, interceptors);
            });
        }

        public static void AddAllEntityAsyncRepositoryScoped<AggregateRoot>(
            this IServiceCollection services,
            Type genericBaseEfRepository)
        {
            var repositories = typeof(AggregateRoot)
            .Assembly.GetTypes()
            .Where(t => typeof(AggregateRoot).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => t);

            foreach (var t in repositories)
            {
                var dataType = new Type[] { t };

                var genericBaseIAsyncRepository = typeof(IAsyncRepository<>);
                var combinedTypeIAsyncRepository = genericBaseIAsyncRepository.MakeGenericType(dataType);
                var combinedTypeEfRepository = genericBaseEfRepository.MakeGenericType(dataType);
                var dataTypes = new Type[] { combinedTypeIAsyncRepository, combinedTypeEfRepository };
                typeof(ServicesExtensions)
                .GetMethod("AddProxiedScoped")
                .MakeGenericMethod(dataTypes)
                .Invoke(services, new object[] { services });
            }
        }
    }
}