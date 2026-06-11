using System.Collections.Generic;
using Godot;

namespace RealFriendlyFarm.Ecs;

public sealed record WorldViewComponent(Node Holder);
public sealed record NodeCollectionComponent(Dictionary<int, Node> Nodes);