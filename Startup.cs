using Core.DependencyInjection.Container;
using Core.DependencyInjection.Core;
using Core.Godot.DependencyInjection.Container;
using Core.Godot.DependencyInjection.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealFriendlyFarm.Assets;

namespace RealFriendlyFarm;

public sealed class Startup : StartupBase
{
    private const string GameContainerPath = "res://GameContainer.tres";
    private const string GameViewContainerPath = "res://GameViewContainer.tres";
    
    protected override void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        
    }

    protected override void ConfigureContainers(IContainerCollection containerCollection)
    {
        containerCollection.RegisterSubContainer<GameContainer>(GameContainerPath);
        containerCollection.RegisterSubContainer<GameViewContainer>(GameViewContainerPath);
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
    }
}