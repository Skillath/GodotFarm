using System;
using Core.DependencyInjection.Host;
using Core.Godot.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Godot.DependencyInjection.Core;

public static class GodotHost
{
    public static IHostBuilder CreateDefaultBuilder()
    {
        return CreateDefaultBuilder(args: Array.Empty<string>(), directory: string.Empty);
    }

    public static IHostBuilder CreateDefaultBuilder(string[] args, string directory)
    {
        var hostBuilder = new ConfigurableHostBuilder()
            //.WithDefaultGameObjectFactory()
             .ConfigureLogging(logger => logger
                .ClearProviders()
                .AddGodotLoggers()
            );
        
        return hostBuilder;
    }
}