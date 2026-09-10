using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Core.Godot.Logging;

[UsedImplicitly]
public static class GodotDefaultLoggerExtensions
{
    [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
    [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
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