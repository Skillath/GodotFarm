using System.Threading;
using System.Threading.Tasks;
using Core.Godot.DependencyInjection.Core;
using Core.MVVM;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using RealFriendlyFarm.Ecs;

namespace RealFriendlyFarm;

[UsedImplicitly]
public sealed class GameHostedService : HostedServiceAsyncBase
{
    private readonly IViewFactory _factory;
    private readonly ILogger<GameHostedService> _logger;

    private IView? _view;

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
        
        _view = _factory.CreateView<WorldViewModel>();
        
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        if (_view is not null)
        {
            _factory.DestroyView(_view);
            _view = null;
        }
        
        _logger.LogInformation("Stopping application");
        
        return Task.CompletedTask;
    }
}