using System.Reflection;
using Core.DependencyInjection.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyInjection.ServiceResolver.Extensions
{
    public static class ServiceResolverExtensions
    {
        public static IServiceCollection UseServiceResolver(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<ServiceResolversLoader>();
        }

        public static IServiceCollection RegisterResolver<TResolver>(this IServiceCollection serviceCollection)
            where TResolver : class, IMemberResolver
        {
            return serviceCollection.AddSingleton<TResolver>();
        }

        public static void UseResolver<TResolver>(this IApplicationBuilder app, MemberTypes memberType)
            where TResolver : IMemberResolver
        {
            var serviceResolverLoader = app.ApplicationServices.GetRequiredService<ServiceResolversLoader>();
            var resolver = app.ApplicationServices.GetRequiredService<TResolver>();

            serviceResolverLoader.RegisterServiceResolver(memberType, resolver);
        }

        internal static void ClearServiceResolver(this IApplicationBuilder app)
        {
            var serviceResolverLoader = app.ApplicationServices.GetRequiredService<ServiceResolversLoader>();
            serviceResolverLoader?.Clear();
        }
    }
}
