#if TOOLS
using System;
using System.Collections.Generic;
using Godot;

namespace SagardoStudios.Foundation;

[Tool]
public partial class FoundationInstaller : EditorPlugin
{
    private const string AddonDir = "res://addons/SagardoStudios.Foundation/";
    private const string MenuLabel = "SagardoStudios.Foundation: Wire solution";

    private static readonly string[] ProjectFiles =
    {
        "Core~/Core.csproj",
        "Core.Godot~/Core.Godot.csproj",
    };

    public override void _EnterTree()
    {
        AddToolMenuItem(MenuLabel, Callable.From(WireSolution));
        CallDeferred(nameof(WireSolution));
    }

    public override void _ExitTree()
    {
        RemoveToolMenuItem(MenuLabel);
    }

    private void WireSolution()
    {
        var assemblyName = ProjectSettings.GetSetting("dotnet/project/assembly_name", "").AsString();
        if (string.IsNullOrEmpty(assemblyName))
            assemblyName = ProjectSettings.GetSetting("application/config/name", "").AsString();

        var slnPath = $"res://{assemblyName}.sln";
        var csprojPath = $"res://{assemblyName}.csproj";
        if (!FileAccess.FileExists(slnPath) || !FileAccess.FileExists(csprojPath))
        {
            Notify("No C# solution found. Run Project > Tools > C# > Create C# solution, then retry.");
            return;
        }

        var slnOs = ProjectSettings.GlobalizePath(slnPath);
        var (listCode, listText) = Run(new[] { "sln", slnOs, "list" });
        if (listCode != 0)
        {
            Notify($"Could not read the solution:\n{listText}");
            return;
        }

        var toAdd = new List<string>();
        foreach (var relative in ProjectFiles)
        {
            if (!listText.Contains(System.IO.Path.GetFileName(relative)))
                toAdd.Add(ProjectSettings.GlobalizePath(AddonDir + relative));
        }

        if (toAdd.Count == 0)
        {
            GD.Print("Foundation: both projects already in the solution.");
            return;
        }

        var addArgs = new List<string> { "sln", slnOs, "add" };
        addArgs.AddRange(toAdd);
        var (addCode, addText) = Run(addArgs.ToArray());
        if (addCode == 0)
            GD.Print($"Foundation: added {string.Join(", ", toAdd)} to {slnPath}.");
        else
            Notify($"Failed to update the solution:\n{addText}");
    }

    private static (int Code, string Text) Run(string[] args)
    {
        var output = new Godot.Collections.Array();
        var code = OS.Execute("dotnet", args, output, true);

        var lines = new List<string>(output.Count);
        foreach (var line in output)
            lines.Add(line.AsString());

        return (code, string.Join("\n", lines));
    }

    private static void Notify(string message)
    {
        GD.PushWarning("SagardoStudios.Foundation: " + message);

        var dialog = new AcceptDialog
        {
            Title = "SagardoStudios.Foundation",
            DialogText = message,
        };
        EditorInterface.Singleton.GetBaseControl().AddChild(dialog);
        dialog.PopupCentered();
        dialog.Confirmed += dialog.QueueFree;
        dialog.Canceled += dialog.QueueFree;
    }
}
#endif
