using Microsoft.Extensions.Logging;

namespace Core.Godot.Logging;

[ProviderAlias("GodotConsole")]
public sealed class GodotLoggerProvider : ILoggerProvider
{
    public void Dispose()
    {
        
    }

    public ILogger CreateLogger(string categoryName)
    {
        return null!;
    }
}