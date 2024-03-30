using Core.DependencyInjection.ServiceResolver;
using Godot;

namespace Core.Godot.MVVM;

public static class PackedSceneExtension
{
    public static Node InstantiateAndResolve(
        this PackedScene packedScene,
        IServiceResolver serviceResolver,
        Node root)
    {
        var node = packedScene.Instantiate();
        serviceResolver.Resolve(node);
        root.CallDeferredThreadGroup("add_child", node);
        return node;
    }
    
    public static TNode InstantiateAndResolve<TNode>(
        this PackedScene packedScene,
        IServiceResolver serviceResolver,
        Node root)
        where TNode : Node
    {
        var node = packedScene.InstantiateAndResolve(serviceResolver, root);
        
        if (node is not TNode instantiatedNode)
            throw new InvalidCastException($"Packed Scene {packedScene} does not match type {typeof(TNode).Name}");
        
        return instantiatedNode;
    }
}