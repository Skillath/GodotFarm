using System.Collections.Generic;
using Godot;

namespace RealFriendlyFarm.Ecs;

public record WorldViewComponent(Node Holder);
public record NodeCollectionComponent(Dictionary<int, Node> Nodes);