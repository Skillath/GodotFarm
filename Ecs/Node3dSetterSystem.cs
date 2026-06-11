using Flecs.NET.Core;
using Godot;
using Microsoft.Extensions.Logging;
using RealFriendlyFarm.Ecs.Component;

namespace RealFriendlyFarm.Ecs;

public sealed class Node3dSetterSystem : ISystem
{
    private readonly WorldProvider _worldProvider;

    public Node3dSetterSystem(WorldProvider worldProvider)
    {
        _worldProvider = worldProvider;
    }

    public void Register()
    {
        _worldProvider.World
            .System<
                Position3dComponent, 
                RotationComponent, 
                ScaleComponent>()
            .Kind(Flecs.NET.Core.Ecs.PostUpdate)
            .Each((
                Entity ent, 
                ref Position3dComponent positionComponent, 
                ref RotationComponent rotationComponent,
                ref ScaleComponent scaleComponent) =>
            {
                var worldNode = _worldProvider.World.Get<WorldViewComponent>().Holder;
                
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

                var node3d = new Node3D
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