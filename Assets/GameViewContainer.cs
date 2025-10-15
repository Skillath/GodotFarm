using Core.DependencyInjection.Core;
using Core.Godot.DependencyInjection.Container;
using Core.Godot.MVVM;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealFriendlyFarm.Assets.Character;
using RealFriendlyFarm.Ecs;

namespace RealFriendlyFarm.Assets;

public sealed partial class GameViewContainer : ResourceContainerBase
{
    [Export] 
    private PackedScene _characterView = default!;

    [Export] 
    private PackedScene _worldView = default!;
    
    public override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IViewCollection, GameViewCollection>();
        
        serviceCollection.AddTransient<CharacterViewModel>();
        serviceCollection.AddTransient<WorldViewModel>();
    }

    public override void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        var viewCollection = app.ApplicationServices.GetRequiredService<IViewCollection>();
        
        viewCollection.RegisterView<CharacterView, CharacterViewModel>(_characterView);
        viewCollection.RegisterView<WorldView, WorldViewModel>(_worldView);    
    }
}