using Core.Godot.DependencyInjection.Core;
using Godot;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace RealFriendlyFarm;

[UsedImplicitly]
public sealed class GameHostedService : HostedServiceAsyncBase
{
    private readonly ILogger<GameHostedService> _logger;

    public GameHostedService(ILogger<GameHostedService> logger)
    {
        _logger = logger;
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(StartAsync));
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(StopAsync));        
        return Task.CompletedTask;
    }
}