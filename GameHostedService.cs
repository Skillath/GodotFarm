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
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting application");
        var view = _factory.CreateView<CharacterViewModel>();

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {      
        _logger.LogInformation("Stopping application");
        return Task.CompletedTask;
    }
}