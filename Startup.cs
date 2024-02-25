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
        throw new NotImplementedException();
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        throw new NotImplementedException();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        throw new NotImplementedException();
    }
}