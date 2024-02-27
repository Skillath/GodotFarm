using Core.DependencyInjection.Container;
using Core.DependencyInjection.Core;
using Core.Godot.MVVM;
using Core.MVVM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RealFriendlyFarm;

public sealed class Startup : IStartup
{
    public void ConfigureContainers(IContainerCollection containerCollection)
    {
        
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IViewFactory, ViewFactory>();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        
    }
}