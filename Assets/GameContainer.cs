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
    }

    public override void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        _ = app.ApplicationServices.GetRequiredService<WorldProvider>();
    }
}