using Microsoft.Extensions.Logging;

namespace Core.Godot.Logging;

public sealed class GodotDefaultLogger : ILogger
{
    private readonly string _name;
    private readonly Func<GodotDefaultLoggerConfiguration> _config;

    public GodotDefaultLogger(string name, Func<GodotDefaultLoggerConfiguration> config)
    {
        _name = name;
        _config = config;
    }
    
    public void Log<TState>(
        LogLevel logLevel, 
        EventId eventId, 
        TState state, 
        Exception? exception, 
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;
        
         var currentConfig = _config.Invoke();
        
         //if (currentConfig.EventId != 0 && currentConfig.EventId != eventId.Id)
         //    return;
        
         var log = currentConfig.LogLevels[logLevel];
        
         var message = $"[{eventId.Id,2}: {logLevel}] - {_name} - {formatter?.Invoke(state, exception)}";
         log.Invoke(message);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _config().LogLevels.ContainsKey(logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default!;
    }
}