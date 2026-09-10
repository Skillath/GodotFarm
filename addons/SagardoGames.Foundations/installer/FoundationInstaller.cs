#if TOOLS
#nullable enable
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Godot;

namespace SagardoGames.Foundations;

[Tool]
public partial class FoundationInstaller : EditorPlugin
{
    private const string FallbackAddonDir = "res://addons/SagardoGames.Foundations";
    private const string MenuLabel = "SagardoGames.Foundations: Regenerate C# solution";
    private const string PropsFile = "Foundation.props";

    private static readonly string[] ProjectFiles =
    {
        ".Core/Core.csproj",
        ".Core.Godot/Core.Godot.csproj",
    };

    private EditorDock? _dock;
    private Label? _status;

    public override void _EnterTree()
    {
        AddToolMenuItem(MenuLabel, Callable.From(RegenerateSolution));
        BuildDock();
        CallDeferred(nameof(RegenerateSolution));
    }

    public override void _ExitTree()
    {
        RemoveToolMenuItem(MenuLabel);
        if (_dock != null)
        {
            RemoveDock(_dock);
            _dock.QueueFree();
            _dock = null;
        }
    }

    private void BuildDock()
    {
        _dock = new EditorDock
        {
            Title = "SagardoGames.Foundations",
            LayoutKey = "SagardoGames.Foundations",
            DefaultSlot = EditorDock.DockSlot.RightUl,
        };

        var content = new VBoxContainer();
        var button = new Button { Text = "Regenerate C# solution" };
        button.Pressed += RegenerateSolution;

        _status = new Label
        {
            AutowrapMode = TextServer.AutowrapMode.Word,
            Text = "Not run yet.",
        };

        content.AddChild(new Label { Text = "SagardoGames.Foundations" });
        content.AddChild(button);
        content.AddChild(_status);
        _dock.AddChild(content);
        AddDock(_dock);
    }

    private void RegenerateSolution()
    {
        var addonDir = ResolveAddonDir();
        var assemblyName = ProjectSettings.GetSetting("dotnet/project/assembly_name", "").AsString();
        if (string.IsNullOrEmpty(assemblyName))
            assemblyName = ProjectSettings.GetSetting("application/config/name", "").AsString();

        var csprojPath = $"res://{assemblyName}.csproj";
        var slnPath = $"res://{assemblyName}.sln";
        if (string.IsNullOrEmpty(assemblyName) || !FileAccess.FileExists(csprojPath) || !FileAccess.FileExists(slnPath))
        {
            SetStatus("No C# solution found. Run Project > Tools > C# > Create C# solution, then press this again.");
            return;
        }

        var importRelative = $"{addonDir.TrimPrefix("res://")}/{PropsFile}";
        string importResult;
        try
        {
            importResult = EnsureImport(ProjectSettings.GlobalizePath(csprojPath), importRelative)
                ? $"Added the <Import> of {PropsFile} to the .csproj."
                : $"{PropsFile} is already imported by the .csproj.";
        }
        catch (Exception e)
        {
            SetStatus($"Failed to edit the .csproj:\n{e.Message}");
            return;
        }

        var solutionResult = EnsureSolutionProjects(ProjectSettings.GlobalizePath(slnPath), addonDir);

        SetStatus(string.Join("\n", importResult, solutionResult, "Rebuild the project for changes to take effect."));
    }

    private string ResolveAddonDir()
    {
        var path = GetScript().As<Script>()?.ResourcePath ?? string.Empty;
        if (string.IsNullOrEmpty(path))
            return FallbackAddonDir;

        // res://addons/SagardoGames.Foundations/installer/FoundationInstaller.cs -> res://addons/SagardoGames.Foundations
        var addonDir = path.GetBaseDir().GetBaseDir();
        return string.IsNullOrEmpty(addonDir) ? FallbackAddonDir : addonDir;
    }

    private static bool EnsureImport(string csprojOsPath, string importRelative)
    {
        var doc = XDocument.Load(csprojOsPath, LoadOptions.PreserveWhitespace);
        var root = doc.Root;
        if (root == null)
            return false;

        foreach (var import in root.Elements("Import"))
        {
            var project = (import.Attribute("Project")?.Value ?? string.Empty).Replace('\\', '/');
            if (project.EndsWith(PropsFile, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        root.Add(new XElement("Import", new XAttribute("Project", importRelative)));
        doc.Save(csprojOsPath, SaveOptions.DisableFormatting);
        return true;
    }

    private string EnsureSolutionProjects(string slnOsPath, string addonDir)
    {
        var (listCode, listText) = Run(new[] { "sln", slnOsPath, "list" });
        if (listCode != 0)
            return $"Could not read the solution:\n{listText}";

        var toAdd = new List<string>();
        foreach (var relative in ProjectFiles)
        {
            if (!listText.Contains(System.IO.Path.GetFileName(relative)))
                toAdd.Add(ProjectSettings.GlobalizePath($"{addonDir}/{relative}"));
        }

        if (toAdd.Count == 0)
            return "Both projects are already in the solution.";

        var addArgs = new List<string> { "sln", slnOsPath, "add", "--solution-folder", "addons" };
        addArgs.AddRange(toAdd);
        var (addCode, addText) = Run(addArgs.ToArray());
        return addCode == 0
            ? $"Added {string.Join(", ", toAdd)} to the solution."
            : $"Failed to update the solution:\n{addText}";
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

    private void SetStatus(string message)
    {
        if (_status != null)
            _status.Text = message;
        GD.Print("Foundation: " + message.Replace("\n", " "));
    }
}
#endif
