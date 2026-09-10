# SagardoStudios.Foundation — Addon Distribution Design

Date: 2026-09-10
Status: Approved (pending implementation plan)

## 1. Problem

`SagardoStudios.Foundation` is a Godot C# addon made of two projects:

- `Core` — plain `Microsoft.NET.Sdk` (net8.0) library: Godot-free abstractions (MVVM, observable, DI).
- `Core.Godot` — Godot-aware library that references `Core` and implements/adapts its abstractions.

Today a consumer must hand-edit their game `.csproj` (two `ProjectReference` lines) and their `.sln` (two project entries). We want a neat, low-friction install that a Godot user can follow, and we want to ship it two ways:

1. **Source addon** — copied under `res://addons/`.
2. **NuGet packages** — `PackageReference` in the game `.csproj`.

## 2. Goals

- Source install: one line in the game `.csproj`, plus one-click sln wiring via an editor plugin.
- NuGet install: one `PackageReference`; the `Core` package flows transitively.
- The addon's Godot version follows the consumer's, driven by an MSBuild property.
- Keep the `Core` / `Core.Godot` separation (Godot-free core stays Godot-free).
- Restructure the addon so Godot's editor never treats the addon's C# as main-assembly scripts.

## 3. Non-goals

- Solving multi-assembly script discovery (engine PR #117452 is unmerged). The addon ships abstract bases and services only; consumers subclass in the main assembly.
- Publishing to nuget.org (phase later; local feed now).
- CI pipeline.

## 4. Key constraint

Godot only auto-discovers C# scripts from the **main** assembly. Types in a referenced assembly are not attachable/`[GlobalClass]` on stock 4.7. This is acceptable because:

- `Core.Godot` exposes abstract Godot types (e.g. `ViewBase<TViewModel> : Node`) and services, not concrete attachable scripts.
- Concrete subclasses live in the consumer's main assembly and are discovered normally.

## 5. Target layout

```
addons/SagardoStudios.Foundation/
  plugin.cfg                              # script = "installer/FoundationInstaller.cs"
  Foundation.props                        # consumer imports this ONE line into game .csproj
  installer/FoundationInstaller.cs        # C# [Tool] EditorPlugin: sln wiring
  Directory.Build.props                   # shared Version / metadata / GodotSharpVersion default
  Core~/Core.csproj                       # Godot-ignored (~), Microsoft.NET.Sdk, packable
  Core.Godot~/Core.Godot.csproj           # Godot-ignored (~), Microsoft.NET.Sdk + GodotSharp
  README.md                               # both install routes
```

Directories ending in `~` are ignored by Godot's filesystem scanner. This keeps the addon's `.cs` out of `res://` script registration while still living physically inside the addon folder for git/update simplicity. The `.cs.uid` files under the C# projects are removed (they are no longer Godot resources).

## 6. Source install

### 6.1 `Foundation.props`

Imported by the consumer's game `.csproj`:

```xml
<Import Project="addons/SagardoStudios.Foundation/Foundation.props" />
```

Contents:

```xml
<Project>
  <ItemGroup>
    <ProjectReference Include="$(MSBuildThisFileDirectory)Core~\Core.csproj"
                      AdditionalProperties="GodotSharpVersion=$(GodotSharpVersion)" />
    <ProjectReference Include="$(MSBuildThisFileDirectory)Core.Godot~\Core.Godot.csproj"
                      AdditionalProperties="GodotSharpVersion=$(GodotSharpVersion)" />
  </ItemGroup>
  <ItemGroup>
    <Compile Remove="$(MSBuildThisFileDirectory)Core~\**\*.cs" />
    <Compile Remove="$(MSBuildThisFileDirectory)Core.Godot~\**\*.cs" />
    <EmbeddedResource Remove="$(MSBuildThisFileDirectory)Core~\**" />
    <EmbeddedResource Remove="$(MSBuildThisFileDirectory)Core.Godot~\**" />
  </ItemGroup>
</Project>
```

- `AdditionalProperties` forwards the consumer's `GodotSharpVersion` into the referenced project builds as a global property.
- The `Compile`/`EmbeddedResource` removals prevent the game project from double-compiling the addon sources (the projects are referenced instead).
- If absolute globs in `Remove` misbehave on a given MSBuild, fall back to `DefaultItemExcludes`.

### 6.2 Godot version override

`Core.Godot` targets `Microsoft.NET.Sdk` explicitly (not `Godot.NET.Sdk`) so its Godot dependency version can come from a property. MSBuild does not permit parameterizing an `Sdk="Godot.NET.Sdk/x.y.z"` attribute by property, hence the switch.

`Core.Godot.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <PackageId>SagardoStudios.Foundation.Core.Godot</PackageId>
    <DefineConstants>GODOT;TOOLS;$(DefineConstants)</DefineConstants>
    <GodotProjectDir Condition="'$(GodotProjectDir)' == ''">$([MSBuild]::GetDirectoryNameOfFileAbove('$(MSBuildThisFileDirectory)', 'project.godot'))</GodotProjectDir>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="GodotSharp" Version="$(GodotSharpVersion)" />
    <PackageReference Include="Godot.SourceGenerators" Version="$(GodotSharpVersion)" PrivateAssets="all" />
    <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="9.0.9" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\Core~\Core.csproj" />
  </ItemGroup>
</Project>
```

- `GODOT;TOOLS` defines are required for source generators' editor-only code (`[Export]` defaults, etc.).
- `GodotProjectDir` is resolved by walking up to `project.godot` so source generators emit correct `res://` paths regardless of where the addon folder sits.
- `GodotSharp` flows to consumers as a package dependency; `Godot.SourceGenerators` is private to the addon build.

`Directory.Build.props` (addon root) provides the default and shared metadata:

```xml
<Project>
  <PropertyGroup>
    <Version>0.1.0</Version>
    <Authors>Xabier Gonzalez Goienetxea</Authors>
    <Company>SagardoStudios</Company>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <GodotSharpVersion Condition="'$(GodotSharpVersion)' == ''">4.7.2</GodotSharpVersion>
    <Nullable>enable</Nullable>
    <LangVersion>latest</LangVersion>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

Consumer override: set `<GodotSharpVersion>4.6.3</GodotSharpVersion>` in their game `.csproj` (or a game-root `Directory.Build.props`). It is forwarded by `Foundation.props`.

## 7. sln wiring — C# installer plugin

Godot requires C# editor plugins to live in the **main** assembly. The addon's `Core~` / `Core.Godot~` sources are excluded from the game compile, so `Foundation.props` excludes only those two folders and leaves `installer/FoundationInstaller.cs` to be globbed into the game (wrapped in `#if TOOLS`). The NuGet route ships no plugin source; `PackageReference` needs no solution wiring.

`plugin.cfg`:

```ini
[plugin]
name="SagardoStudios.Foundation"
description="Foundation for Godot C# projects (MVVM, DI, logging)."
author="Xabier Gonzalez Goienetxea"
version="0.1"
script="installer/FoundationInstaller.cs"
```

`installer/FoundationInstaller.cs` — `[Tool] partial class FoundationInstaller : EditorPlugin` (inside `#if TOOLS`):

- On `_EnterTree()` and from a menu item, wire the solution:
  1. Resolve the game assembly name from `ProjectSettings` (`dotnet/project/assembly_name`) and locate `<name>.sln` / `<name>.csproj` at `res://` root.
  2. Run `OS.Execute("dotnet", ["sln", <sln>, "add", <Core.csproj>, <Core.Godot.csproj>], output, true)`.
  3. Report success/failure to the Output panel and a dialog.
- Idempotent: only adds projects not already listed (`dotnet sln list` first, or tolerate "already" output).
- Does **not** edit the `.csproj`; the props import remains a manual one-liner.
- Handles the "no C# solution yet" case with a clear message pointing at `Project ▸ Tools ▸ C# ▸ Create C# solution`.
- The class name has no dot and matches the file name, so Godot's `ScriptPathAttribute` maps `res://addons/SagardoStudios.Foundation/installer/FoundationInstaller.cs` to the type.

Rationale: `dotnet sln add` is the standard, robust way to edit a solution. Hand-parsing the `.sln` format is avoided.

## 8. NuGet install

- `Core.csproj` → `PackageId=SagardoStudios.Foundation.Core`.
- `Core.Godot.csproj` → `PackageId=SagardoStudios.Foundation.Core.Godot`; its `ProjectReference` to `Core` becomes a same-version package dependency at pack time.
- Consumer adds one reference:

```xml
<PackageReference Include="SagardoStudios.Foundation.Core.Godot" Version="0.1.0" />
```

- Local feed for development: `dotnet pack addons/SagardoStudios.Foundation/Core.Godot~/Core.Godot.csproj -c Release -o artifacts`, then a `nuget.config` pointing at `artifacts/` (or `dotnet nuget add source`).
- The package's `GodotSharp` dependency is pinned to `$(GodotSharpVersion)` at pack time. Consumers on a different Godot minor may need to align versions; documented as a compatibility note.

## 9. Cleanup / migration

- Delete `Core/SagardoStudios.Foundation.cs` — it references Godot inside the Godot-free `Core`, uses an invalid class name (`SagardoStudios.Foundation` contains a dot), and its path disagrees with `plugin.cfg`.
- Move `Core/` → `Core~/` and `Core.Godot/` → `Core.Godot~/`.
- Remove `.uid` files under the moved C# projects; remove `Core.Godot~/.godot` build artifacts.
- Update `RealFriendlyFarm.csproj` and `RealFriendlyFarm.sln` to use the new route (replace manual `ProjectReference`s with the `Foundation.props` import), validating the consumer experience on the current game.

## 10. Risks

| Risk | Mitigation |
|------|------------|
| `Microsoft.NET.Sdk` project lacks Godot SDK behaviors (defines, generators, output paths) | Add `GODOT;TOOLS`, `Godot.SourceGenerators`, `GodotProjectDir`; verify build + runtime. |
| `~` folder breaks tooling or packaging | `~` is a Godot ignore convention; MSBuild/NuGet handle it. Verified by build/pack. |
| Absolute glob in `Compile Remove` unsupported | Fallback to `DefaultItemExcludes`. |
| Source-addon consumers on a different Godot minor | `GodotSharpVersion` property override; document compatibility. |
| `dotnet` not on PATH when the plugin runs | Detect and show a clear error; sln wiring stays optional (build still works without it). |
| Multi-assembly script discovery | Addon exposes abstract bases/services only; documented. |

## 11. Verification

- `dotnet build RealFriendlyFarm.sln` succeeds after migration.
- `dotnet pack addons/SagardoStudios.Foundation/Core.Godot~/Core.Godot.csproj -c Release -o artifacts` produces `SagardoStudios.Foundation.Core` and `SagardoStudios.Foundation.Core.Godot` `.nupkg`.
- (Manual, no Godot binary in this environment) Enable the plugin in the editor; confirm sln gains both projects; confirm no C# script-scan errors for addon sources.
- Optional: throwaway consumer project that installs the local package and builds.

## 12. Phasing (single plan, ordered)

1. Restructure folders (`Core~`, `Core.Godot~`), delete broken plugin `.cs`, drop `.uid`.
2. Add `Directory.Build.props`; convert `Core.Godot` to `Microsoft.NET.Sdk` + GodotSharp.
3. Add `Foundation.props`; rewire `RealFriendlyFarm.csproj`; verify build.
4. Add `installer/FoundationInstaller.cs` + `plugin.cfg`; wire sln.
5. Add NuGet metadata; pack to `artifacts/`; verify packages and dependencies.
6. Write `README.md` (both routes) and compatibility notes.
