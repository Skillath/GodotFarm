# SagardoGames.Foundations

A small foundation for Godot C# projects: MVVM view bases, an observable/binding
layer, a dependency-injection host built on `Microsoft.Extensions.*`, and Godot
logging.

It ships as two projects:

- **Core** (`SagardoGames.Foundations.Core`) — Godot-agnostic abstractions.
- **Core.Godot** (`SagardoGames.Foundations.Core.Godot`) — Godot integration. Depends on Core.

## Requirements

- .NET 8 SDK
- Godot 4.x with C# support. Defaults to `Godot.NET.Sdk` / `GodotSharp` **4.7.2**.

## Install — source addon (recommended)

1. Download `SagardoGames.Foundations-<version>.zip` from the
   [latest release](../../releases/latest) and extract it into your project root.
   The archive root is `addons/`, so `addons/SagardoGames.Foundations/` lands in place.

2. Add exactly one line to your game `.csproj`:

   ```xml
   <Import Project="addons/SagardoGames.Foundations/Foundation.props" />
   ```

3. Build (`dotnet build` or the Godot editor's **Build** button).

4. Optional — add the two projects to your `.sln` and (re)assert the import:
   enable the plugin in **Project Settings → Plugins**. A
   **SagardoGames.Foundations** dock appears on the right with a
   **Regenerate C# solution** button. It:
   - ensures the `<Import>` of `Foundation.props` is in your `.csproj` (idempotent),
   - adds `.Core` and `.Core.Godot` to your `.sln` via `dotnet sln add` (idempotent).

   The same action is available from **Project → SagardoGames.Foundations: Regenerate C# solution**.

> Godot's own **Project > Tools > C# > Create C# solution** regenerates the `.csproj` from
> scratch and **wipes custom content** (the import and your packages). After using it, press
> **Regenerate C# solution** to restore the addon wiring. To bootstrap the plugin the first
> time, the import must be present and the project built once (C# plugins live in your main
> assembly).
>
> The editor plugin is C# (`installer/FoundationInstaller.cs`), so its source compiles into
> your game's main assembly, as Godot requires. `Foundation.props` references only the two
> dot-prefixed project folders (hidden from Godot and the default MSBuild glob), leaving the
> installer in the main assembly. On the NuGet route there is no plugin source; a
> `PackageReference` needs no solution wiring.

### Choosing your Godot version

`Foundation.props` forwards a `GodotSharpVersion` property into both projects. If
you are not on 4.7.2, set it in your game `.csproj`:

```xml
<PropertyGroup>
  <GodotSharpVersion>4.6.3</GodotSharpVersion>
</PropertyGroup>
```

It must match the `Godot.NET.Sdk` version your game uses.

## Install — NuGet package

Each [release](../../releases/latest) attaches `SagardoGames.Foundations.Core` and
`SagardoGames.Foundations.Core.Godot` as `.nupkg` files. `Core` flows in
transitively.

1. Download both `.nupkg` files into a local folder, e.g. `./packages-local`.

2. Point a package source at that folder (`nuget.config` at your project root):

   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <packageSources>
       <add key="local" value="./packages-local" />
     </packageSources>
   </configuration>
   ```

3. Reference the Godot package; `Core` flows in transitively:

   ```xml
   <PackageReference Include="SagardoGames.Foundations.Core.Godot" Version="0.1.0" />
   ```

To build the packages yourself:

```bash
dotnet pack addons/SagardoGames.Foundations/.Core/Core.csproj -c Release -o artifacts
dotnet pack addons/SagardoGames.Foundations/.Core.Godot/Core.Godot.csproj -c Release -o artifacts
```

## Project layout

```
addons/SagardoGames.Foundations/
  plugin.cfg                     editor plugin metadata
  Foundation.props               imported by the game csproj
  installer/FoundationInstaller.cs   wires the .sln (C# editor plugin)
  Directory.Build.props          version, metadata, GodotSharpVersion default
  .Core/                         C# source (Godot-agnostic; dot-prefixed, hidden from Godot)
  .Core.Godot/                   C# source (Godot integration; dot-prefixed)
```

Dot-prefixed folders are ignored by Godot's filesystem scanner, so the addon's C#
sources are never treated as main-assembly scripts.

## Releasing

Push a tag `vX.Y.Z`. `.github/workflows/release.yml` then:

1. asserts `plugin.cfg` `version` equals the tag,
2. packs `SagardoGames.Foundations.Core` and `SagardoGames.Foundations.Core.Godot`
   with that version,
3. builds `SagardoGames.Foundations-X.Y.Z.zip` (archive root `addons/`),
4. attaches the zip and both `.nupkg` files to the GitHub release.

Before tagging, bump the version in **both** places:

- `addons/SagardoGames.Foundations/plugin.cfg` → `version`
- `addons/SagardoGames.Foundations/Directory.Build.props` → `<Version>`

No repository secrets required.

## Compatibility note

Godot only auto-discovers C# scripts from the main assembly. This addon exposes
abstract Godot types (e.g. `ViewBase<TViewModel>`) and services; put your
concrete subclasses in your game's own assembly and they are discovered
normally.

## License

MIT — see `LICENSE`.
