using System.Linq;
using Flecs.NET.Core;
using Godot;
using RealFriendlyFarm.Ecs.Component;

namespace RealFriendlyFarm.Ecs;

public sealed class GameObjectSpawnSystem : ISystem
{
    private readonly WorldProvider _worldProvider;

    public GameObjectSpawnSystem(WorldProvider worldProvider)
    {
        _worldProvider = worldProvider;
    }

    public void Register()
    {
        ref var world = ref _worldProvider.World;
        var worldNode = world.Get<WorldViewComponent>().Holder;
        var nodeCollection = world.Get<NodeCollectionComponent>().Nodes;
        
        _worldProvider.World
            .System<
                PositionComponent, 
                RotationComponent, 
                ScaleComponent>()
            .Each((
                Entity ent, 
                ref PositionComponent positionComponent, 
                ref RotationComponent rotationComponent,
                ref ScaleComponent scaleComponent) =>
            {
                var name = ent.Name();

                for (var i = 0; i < worldNode.GetChildCount(); i++)
                {
                    var childNode = worldNode.GetChild(i);
                    if (childNode is not Node3D childNode3D || childNode.Name != name) 
                        continue;
                    
                    childNode3D.Position = new Vector3(positionComponent.X, positionComponent.Y, positionComponent.Z);
                    childNode3D.RotationDegrees = new Vector3(rotationComponent.X, rotationComponent.Y, rotationComponent.Z);
                    childNode3D.Scale = new Vector3(scaleComponent.X, scaleComponent.Y, scaleComponent.Z);    
                    return;
                }
                
                if (worldNode.GetChildren().Any(c => c.Name == name))
                    return;

                var node3d = new Node3D()
                {
                    Name = name,
                    Position = new Vector3(positionComponent.X, positionComponent.Y, positionComponent.Z),
                    Scale = new Vector3(scaleComponent.X, scaleComponent.Y, scaleComponent.Z),
                    RotationDegrees = new Vector3(rotationComponent.X, rotationComponent.Y, rotationComponent.Z),
                };
                
                worldNode.AddChild(node3d);
            });
    }
}