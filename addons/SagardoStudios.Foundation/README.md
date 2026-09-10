# SagardoStudios.Foundation

A small foundation for Godot C# projects: MVVM view bases, an observable/binding
layer, a dependency-injection host built on `Microsoft.Extensions.*`, and Godot
logging.

It ships as two projects:

- **Core** (`SagardoStudios.Foundation.Core`) — Godot-agnostic abstractions.
- **Core.Godot** (`SagardoStudios.Foundation.Core.Godot`) — Godot integration. Depends on Core.

## Requirements

- .NET 8 SDK
- Godot 4.x with C# support. Defaults to `Godot.NET.Sdk` / `GodotSharp` **4.7.2**.

## Install — source addon (recommended)

1. Copy the `SagardoStudios.Foundation/` folder into your project's `res://addons/`.

2. Add exactly one line to your game `.csproj`:

   ```xml
   <Import Project="addons/SagardoStudios.Foundation/Foundation.props" />
   ```

3. Build (`dotnet build` or the Godot editor's **Build** button).

4. Optional — add the two projects to your `.sln` so your IDE shows them:
   enable the plugin in **Project Settings → Plugins**, or run
   **Project → SagardoStudios.Foundation: Wire solution**. This calls
   `dotnet sln add`; it is idempotent.

> The editor plugin is C#, so its source (`installer/FoundationInstaller.cs`)
> compiles into your game's main assembly, as Godot requires for C# plugins.
> `Foundation.props` excludes only the two `~` project folders, leaving the
> installer in the main assembly. On the NuGet route there is no plugin source;
> a `PackageReference` needs no solution wiring.

### Choosing your Godot version

`Foundation.props` forwards a `GodotSharpVersion` property into both projects. If
you are not on 4.7.2, set it in your game `.csproj`:

```xml
<PropertyGroup>
  <GodotSharpVersion>4.6.3</GodotSharpVersion>
</PropertyGroup>
```

It must match the `Godot.NET.Sdk` version your game uses.

## Install — NuGet

Add a single package reference; `Core` flows in transitively:

```xml
<PackageReference Include="SagardoStudios.Foundation.Core.Godot" Version="0.1.0" />
```

For local development, pack and consume from a folder feed:

```bash
dotnet pack addons/SagardoStudios.Foundation/Core~/Core.csproj -c Release -o artifacts
dotnet pack addons/SagardoStudios.Foundation/Core.Godot~/Core.Godot.csproj -c Release -o artifacts
```

## Project layout

```
addons/SagardoStudios.Foundation/
  plugin.cfg                     editor plugin metadata
  Foundation.props               imported by the game csproj
  installer/FoundationInstaller.cs   wires the .sln (C# editor plugin)
  Directory.Build.props          version, metadata, GodotSharpVersion default
  Core~/                         C# source (Godot ignores folders ending in ~)
  Core.Godot~/
```

`~` folders are ignored by Godot's filesystem scanner, so the addon's C# sources
are never treated as main-assembly scripts.

## Compatibility note

Godot only auto-discovers C# scripts from the main assembly. This addon exposes
abstract Godot types (e.g. `ViewBase<TViewModel>`) and services; put your
concrete subclasses in your game's own assembly and they are discovered
normally.

## License

MIT — see `license.md`.
