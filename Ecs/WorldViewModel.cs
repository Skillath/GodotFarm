using Core.MVVM;
using Flecs.NET.Core;
using JetBrains.Annotations;
using RealFriendlyFarm.Ecs.Component;

namespace RealFriendlyFarm.Ecs;

[UsedImplicitly]
public sealed class WorldViewModel : ViewModelBase
{
    private readonly WorldProvider _worldProvider;

    public ref World World => ref _worldProvider.World;

    public WorldViewModel(WorldProvider worldProvider)
    {
        _worldProvider = worldProvider;

        for (var i = 0; i < 1000; i++)
        {
            World.Entity($"TestNode{i}")
                .Set(new PositionComponent(0, 0, 0))
                .Set(new RotationComponent(0, 0, 0))
                .Set(new ScaleComponent(1, 1, 1));   
        }
    }
}