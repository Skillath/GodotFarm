using System.Collections.Concurrent;
using System.Diagnostics;
using Godot;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Core.Godot.Logging;

[ProviderAlias("GodotConsole")]
public sealed class GodotDefaultLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private readonly ConcurrentDictionary<string, ILogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
    
    private GodotDefaultLoggerConfiguration _currentConfig;
    
    
    public GodotDefaultLoggerProvider(IOptionsMonitor<GodotDefaultLoggerConfiguration> config)
    {
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updateConfig => _currentConfig = updateConfig);
    }
    
    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new GodotDefaultLogger(name, () => _currentConfig));
    }
}

public sealed class GodotDefaultLogger : ILogger
{
    private readonly string _name;
    private readonly Func<GodotDefaultLoggerConfiguration> _config;
    //private readonly UnityEngine.ILogger _logger;

    public GodotDefaultLogger(string name, Func<GodotDefaultLoggerConfiguration> config)
    {
        _name = name;
        _config = config;
        //_logger = Debug.unityLogger;
    }
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;
        
        // var currentConfig = _config();
        //
        // //if (currentConfig.EventId != 0 && currentConfig.EventId != eventId.Id)
        // //    return;
        //
        // var log = currentConfig.LogLevels[logLevel];
        //
        // var message = $"[{eventId.Id,2}: {logLevel}] - {_name} - {formatter?.Invoke(state, exception)}";
        // log.Invoke(message);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        //return _logger.logEnabled && _config().LogLevels.ContainsKey(logLevel);
        return _config().LogLevels.ContainsKey(logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default!;
    }
}

public sealed class GodotDefaultLoggerConfiguration
{
    public int EventId { get; set; } = -1;

    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    public IReadOnlyDictionary<LogLevel, Action<object>> LogLevels { get; set; } = new Dictionary<LogLevel, Action<object>>()
    {
        //[LogLevel.Information] = Debug.Log,
        //[LogLevel.Trace] = Debug.Log,
        //[LogLevel.Debug] = Debug.Log,
        //[LogLevel.Warning] = Debug.LogWarning,
        //[LogLevel.Error] = Debug.LogError,
        //[LogLevel.Critical] = Debug.LogError,
    };
}

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