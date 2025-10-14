using Flecs.NET.Core;
using JetBrains.Annotations;

namespace RealFriendlyFarm.Ecs;

[UsedImplicitly]
public sealed class WorldProvider
{
    private World _world = World.Create();

    public ref World World => ref _world;
}