using AsyncAwaitBestPractices;
using Core.DependencyInjection.Container;
using Core.DependencyInjection.Container.Extensions;
using Core.DependencyInjection.Core;
using Core.DependencyInjection.Exceptions;
using Core.DependencyInjection.ServiceResolver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.DependencyInjection.Host.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseStartup<TStartup>(this IHostBuilder hostBuilder)
        where TStartup : class, IStartup
    {
        return hostBuilder.ConfigureServices((hostContext, services) =>
        {
            var startup = Activator.CreateInstance<TStartup>();

            if (services.Any(sd => sd.ImplementationType == typeof(IStartup)))
                throw new DependencyInjectionException("Startup already declared");

            var containerCollection = new ContainerCollection();
            containerCollection.RegisterSubContainer<ServiceResolverContainer>();
            startup.ConfigureContainers(containerCollection);

            services.AddSingleton<IStartup>(startup);

            services.AddSingleton<IContainerCollection>(containerCollection);
            services.AddSingleton<IApplicationBuilder, ApplicationBuilder>(serviceProvider => 
                new ApplicationBuilder(serviceProvider));

            containerCollection.ConfigureServices(services);
            startup.ConfigureServices(services);

            // hostBuilder.Configure(containerCollection.Configure);
            // hostBuilder.Configure(startup.Configure);
        });
    }

    public static IHostBuilder UseHostedService<THostedService>(this IHostBuilder hostBuilder)
        where THostedService : class, IHostedService
    {
        return hostBuilder.ConfigureServices((hostContext, services) => services.AddHostedService<THostedService>());
    }

    public static IHostBuilder Configure(this IHostBuilder hostBuilder, Action<IApplicationBuilder, IHostEnvironment> configure)
    {
        if (hostBuilder is not IConfigurable configurable)
            throw new DependencyInjectionException($"You only can use the {nameof(Configure)} method on a {nameof(IConfigurable)} {nameof(IHostBuilder)}");
            
        configurable.AddConfigure(configure);

        return hostBuilder;
    }

    public static IHost BuildAndRunConfigurableHostAsync(
        this IHostBuilder hostBuilder, 
        CancellationToken cancellationToken)
    {
        var host = hostBuilder.BuildConfigurableHost();

        host.RunAsync(cancellationToken).SafeFireAndForget();

        return host;
    }

    public static IHost BuildConfigurableHost(this IHostBuilder? hostBuilder)
    {
        var host = hostBuilder?.Build()
                   ?? throw new DependencyInjectionException($"Host is null. Maybe the HostBuilder you're using is null");

        if (hostBuilder is not IConfigurable configurable)
            throw new DependencyInjectionException($"You only can use the {nameof(BuildConfigurableHost)} method on a {nameof(IConfigurable)} {nameof(IHostBuilder)}");

        var services = host.Services;
            
        var env = services.GetRequiredService<IHostEnvironment>();
        var app = services.GetRequiredService<IApplicationBuilder>();

        foreach (var configure in configurable.ConfigureCollection)
        {
            configure?.Invoke(app, env);
        }

        return host;
    }
}