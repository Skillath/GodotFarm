using Core.DependencyInjection.Container;
using Core.DependencyInjection.Core;
using Godot;
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
        
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        
    }
}