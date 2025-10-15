using System.Threading;
using System.Threading.Tasks;
using Core.Godot.DependencyInjection.Core;
using Core.MVVM;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using RealFriendlyFarm.Assets.Character;
using RealFriendlyFarm.Ecs;

namespace RealFriendlyFarm;

[UsedImplicitly]
public sealed class GameHostedService : HostedServiceAsyncBase
{
    private readonly WorldProvider _worldProvider;
    private readonly IViewFactory _factory;
    private readonly ILogger<GameHostedService> _logger;

    public GameHostedService(
        WorldProvider worldProvider,
        IViewFactory factory, 
        ILogger<GameHostedService> logger)
    {
        _worldProvider = worldProvider;
        _factory = factory;
        _logger = logger;
        
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting application");
        var view = _factory.CreateView<WorldViewModel>();
        
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {      
        _logger.LogInformation("Stopping application");
        return Task.CompletedTask;
    }
}