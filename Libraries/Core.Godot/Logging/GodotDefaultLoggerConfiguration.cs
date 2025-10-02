using System;
using System.Collections.Generic;
using Godot;
using Microsoft.Extensions.Logging;

namespace Core.Godot.Logging;

public sealed class GodotDefaultLoggerConfiguration
{
    public int EventId { get; set; } = -1;

    public LogLevel LogLevel { get; set; } = LogLevel.Debug;

    public IReadOnlyDictionary<LogLevel, Action<object>> LogLevels { get; } = new Dictionary<LogLevel, Action<object>>
    {
        [LogLevel.Information] = obj => GD.PrintRich(obj.ToString()),
        [LogLevel.Trace] = obj => GD.PrintRich(obj.ToString()),
        [LogLevel.Debug] = obj => GD.PrintRich(obj.ToString()),
        [LogLevel.Warning] = PrintWarning,
        [LogLevel.Error] = PrintError,
        [LogLevel.Critical] = PrintError,
    };

    private static void PrintWarning(object obj)
    {
        GD.PrintRich($"[color=yellow]{obj}[/color]");
        GD.PushWarning(obj.ToString());
    }

    private static void PrintError(object obj)
    {
        GD.PrintRich($"[color=red]{obj}[/color]");
        GD.PushError(obj.ToString());
    }
}