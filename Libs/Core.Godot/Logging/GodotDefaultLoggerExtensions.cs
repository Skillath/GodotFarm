using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Core.Godot.Logging;

[UsedImplicitly]
public static class GodotDefaultLoggerExtensions
{
    public static ILoggingBuilder AddGodotLoggers(this ILoggingBuilder builder)
    {
        builder.ClearProviders();
        builder.AddConfiguration();
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, GodotDefaultLoggerProvider>());
        LoggerProviderOptions.RegisterProviderOptions<GodotDefaultLoggerConfiguration, GodotDefaultLoggerProvider>(builder.Services);
        return builder;
    }

    public static ILoggingBuilder AddGodotLoggers(this ILoggingBuilder builder, Action<GodotDefaultLoggerConfiguration> configure)
    {
        builder.AddGodotLoggers();
        builder.Services.Configure(configure);

        return builder;
    }
}