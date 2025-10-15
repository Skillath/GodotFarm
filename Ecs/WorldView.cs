using System.Collections.Generic;
using Core.Godot.MVVM;
using Flecs.NET.Core;
using Godot;

namespace RealFriendlyFarm.Ecs;

public sealed partial class WorldView : ViewBase<WorldViewModel>
{
    private ref World World => ref ViewModel.World;
    
    protected override void Bind()
    {
        World
            .Set(new WorldViewComponent(this))
            .Set(new NodeCollectionComponent(new Dictionary<int, Node>()));
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        World.Progress((float)delta);
    }
}