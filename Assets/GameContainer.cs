using Core.DependencyInjection.Core;
using Core.Godot.DependencyInjection.Container;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RealFriendlyFarm.Assets;

public sealed partial class GameContainer : ResourceContainerBase
{
    public override void ConfigureServices(IServiceCollection serviceCollection)
    {
        
    }

    public override void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        
    }
}