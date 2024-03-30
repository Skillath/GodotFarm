using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
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