using Core.DependencyInjection.Core;
using Core.DependencyInjection.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.DependencyInjection.Container;

public sealed class ContainerCollection : IContainerCollection
{
    private readonly ICollection<IContainer> _subContainers = new HashSet<IContainer>();

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        foreach (var subContainer in _subContainers)
        {
            subContainer.Configure(app, env);
        }
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        foreach (var subContainer in _subContainers)
        {
            subContainer.ConfigureServices(serviceCollection);
        }
    }

    public IContainerCollection RegisterSubContainer(IContainer container)
    {
        if (container is null)
        {
            throw new DependencyInjectionException($"The Container can't be null");
        }

        if (_subContainers.Contains(container))
        {
            throw new DependencyInjectionException($"This container instance [{container?.GetType().Name}] already has been registered");
        }

        _subContainers.Add(container);
        return this;
    }

    public void Dispose()
    {
        _subContainers?.Clear();
    }
}