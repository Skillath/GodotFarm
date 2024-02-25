using Godot;
using Microsoft.Extensions.Logging;

namespace Core.Godot.Logging;

public sealed class GodotDefaultLoggerConfiguration
{
    public int EventId { get; set; } = -1;

    public LogLevel LogLevel { get; set; } = LogLevel.Information;

    public IReadOnlyDictionary<LogLevel, Action<object>> LogLevels { get; set; } = new Dictionary<LogLevel, Action<object>>()
    {
        [LogLevel.Information] = obj => GD.Print(obj.ToString()),
        [LogLevel.Trace] = obj => GD.Print(obj.ToString()),
        [LogLevel.Debug] = obj => GD.Print(obj.ToString()),
        [LogLevel.Warning] = obj => GD.PushWarning(obj.ToString()),
        [LogLevel.Error] = obj => GD.PushError(obj.ToString()),
        [LogLevel.Critical] = obj => GD.PushError(obj.ToString()),
    };
}