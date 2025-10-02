using Core.DependencyInjection.Container;
using Core.DependencyInjection.Exceptions;
using Godot;

namespace Core.Godot.DependencyInjection.Container;

public static class ContainerCollectionExtension
{
    public static IContainerCollection RegisterSubContainer<TContainer>(
        this IContainerCollection containerCollection,
        string path)
        where TContainer : ResourceContainerBase
    {
        var type = typeof(TContainer);

        if (type.IsInterface)
            throw new DependencyInjectionException($"Type {type.FullName} can't be an interface!");

        if (type.IsAbstract)
            throw new DependencyInjectionException($"Type {type.FullName} can't be abstract!");

        var container = GD.Load<TContainer>(path);
        return containerCollection.RegisterSubContainer(container);
    }
}