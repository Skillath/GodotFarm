using System.Reflection;
using Core.DependencyInjection.Container;
using Core.DependencyInjection.Core;
using Core.DependencyInjection.ServiceResolver.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.DependencyInjection.ServiceResolver
{
    public sealed class ServiceResolverContainer : IContainer
    {
        private IApplicationBuilder? _app;

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.UseServiceResolver();

            serviceCollection.RegisterResolver<FieldResolver>();
            serviceCollection.RegisterResolver<PropertyResolver>();
            serviceCollection.RegisterResolver<MethodResolver>();
            serviceCollection.RegisterResolver<ConstructorResolver>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment _)
        {
            _app = app 
                ?? throw new ArgumentNullException(nameof(app));

            app.UseResolver<FieldResolver>(MemberTypes.Field);
            app.UseResolver<PropertyResolver>(MemberTypes.Property);
            app.UseResolver<MethodResolver>(MemberTypes.Method);
            app.UseResolver<ConstructorResolver>(MemberTypes.Constructor);
        }

        public void Dispose()
        {
            _app?.ClearServiceResolver();
        }
    }
}