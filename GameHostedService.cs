using Core.Godot.DependencyInjection.Core;
using Core.MVVM;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealFriendlyFarm.Assets;

namespace RealFriendlyFarm;

[UsedImplicitly]
public sealed class GameHostedService : HostedServiceAsyncBase, IHostedService
{
    private readonly IViewFactory _factory;
    private readonly ILogger<GameHostedService> _logger;

    public GameHostedService(
        IViewFactory factory, 
        ILogger<GameHostedService> logger)
    {
        _factory = factory;
        _logger = logger;
    }
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(500, cancellationToken);
        _logger.LogInformation("Information");
        await Task.Delay(500, cancellationToken);
        _logger.LogWarning("Warning");
        await Task.Delay(500, cancellationToken);
        _logger.LogError("Error");
        await Task.Delay(500, cancellationToken);
        _logger.LogCritical("Critical");

        var view = _factory.CreateView<CharacterViewModel>();
        
        
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {      
        return Task.CompletedTask;
    }
}