using Core.DependencyInjection.Core;
using Core.Godot.DependencyInjection.Container;
using Core.Godot.MVVM;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RealFriendlyFarm.Assets;

public sealed partial class GameViewContainer : ResourceContainerBase
{
    [Export] 
    private PackedView<CharacterView> _characterView = default!;

    public override void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IViewCollection, GameViewCollection>();
        serviceCollection.AddTransient<CharacterViewModel>();
    }

    public override void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        var viewCollection = app.ApplicationServices.GetRequiredService<IViewCollection>();
        //viewCollection.RegisterView<CharacterView, CharacterViewModel>(_characterView);
    }
}