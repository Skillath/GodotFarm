using Core.DependencyInjection.Container;
using Core.DependencyInjection.Container.Extensions;
using Core.DependencyInjection.Core;
using Core.Godot.MVVM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Godot.DependencyInjection.Core;

public abstract class StartupBase : IStartup
{
    protected abstract void ConfigureContainers(IContainerCollection containerCollection);

    protected abstract void ConfigureServices(IServiceCollection serviceCollection);

    protected abstract void Configure(IApplicationBuilder app, IHostEnvironment env);
    
    void IStartup.ConfigureContainers(IContainerCollection containerCollection)
    {
        containerCollection.RegisterSubContainer<MvvmContainer>();
        ConfigureContainers(containerCollection);
    }

    void IStartup.ConfigureServices(IServiceCollection serviceCollection)
    {
        ConfigureServices(serviceCollection);
    }

    void IStartup.Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        Configure(app, env);
    }
}