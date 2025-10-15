using System.Collections.Generic;
using Core.DependencyInjection.Core;
using Core.Godot.DependencyInjection.Container;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealFriendlyFarm.Ecs;

namespace RealFriendlyFarm.Assets;

public sealed partial class GameContainer : ResourceContainerBase
{
    public override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<WorldProvider>();
        serviceCollection.AddSingleton<ISystem, GameObjectSpawnSystem>();
    }

    public override void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        var systems = app.ApplicationServices.GetRequiredService<IEnumerable<ISystem>>();
        foreach (var system in systems)
        {
            system.Register();
        }
    }
}