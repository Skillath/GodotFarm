using Core.Godot.DependencyInjection.Core;
using Core.MVVM;
using Godot;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace RealFriendlyFarm;

[UsedImplicitly]
public sealed class GameHostedService : HostedServiceAsyncBase
{
    

    public GameHostedService(IViewFactory factory)
    {
        GD.Print("Hello");
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {      
        return Task.CompletedTask;
    }
}