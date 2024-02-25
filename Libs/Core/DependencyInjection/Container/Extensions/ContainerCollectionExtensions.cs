using Core.DependencyInjection.Exceptions;

namespace Core.DependencyInjection.Container.Extensions;

public static class ContainerCollectionExtensions
{
    public static IContainerCollection RegisterSubContainer<TContainer>(this IContainerCollection containerCollection)
        where TContainer : class, IContainer, new()
    {
        var type = typeof(TContainer);

        if (type.IsInterface)
            throw new DependencyInjectionException($"Type {type.FullName} can't be an interface!");

        if (type.IsAbstract)
            throw new DependencyInjectionException($"Type {type.FullName} can't be abstract!");
            
        return containerCollection.RegisterSubContainer(Activator.CreateInstance<TContainer>());
    }
}