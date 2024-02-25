using Core.Godot.DependencyInjection.Core;
using Godot;
using JetBrains.Annotations;

namespace RealFriendlyFarm;

[UsedImplicitly]
public sealed class GameHostedService : HostedServiceAsyncBase
{
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        GD.Print(nameof(StartAsync));
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        GD.Print(nameof(StopAsync));        
        return Task.CompletedTask;
    }
}