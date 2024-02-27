using Core.DependencyInjection.Host;
using Core.Godot.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Godot.DependencyInjection.Core;

public static class GodotHost
{
    public static IHostBuilder CreateDefaultBuilder() =>
        CreateDefaultBuilder(args: Array.Empty<string>(), directory: string.Empty);

    public static IHostBuilder CreateDefaultBuilder(string[] args, string directory)
    {
        var hostBuilder = new ConfigurableHostBuilder()
            //.WithDefaultGameObjectFactory()
             .ConfigureLogging(logger => 
                 logger.ClearProviders().AddGodotLoggers());

        //TODO: Add support for appsettings.json and so on

        return hostBuilder;
    }

    /*public static IHostBuilder WithDefaultGameObjectFactory(this IHostBuilder hostBuilder)
    {
        return hostBuilder.WithCustomDefaultGameObjectFactory<GameObjectFactory>();
    }

    public static IHostBuilder WithCustomDefaultGameObjectFactory<TFactory>(this IHostBuilder hostBuilder)
        where TFactory : class, IFactory<GameObject>
    {
        return hostBuilder
            .ConfigureServices(services =>
            {
                var descriptor = ServiceDescriptor.Singleton(s => s.GetService<IFactory<GameObject>>());

                if (services.Contains(descriptor))
                    services.Remove(descriptor);

                services.AddSingleton<IFactory<GameObject>, TFactory>();
            });
    }*/
}