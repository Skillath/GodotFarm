using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace Core.Godot.DependencyInjection.Core;

[UsedImplicitly]
public abstract class HostedServiceBase : IHostedService
{
    private readonly IHostApplicationLifetime _appLifetime;

    protected HostedServiceBase(IHostApplicationLifetime appLifetime)
    {
        _appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (_appLifetime.ApplicationStarted.Register(OnStarted))
        using (_appLifetime.ApplicationStopping.Register(OnStopping))
        using (_appLifetime.ApplicationStopped.Register(OnStopped))
        {
            //Does this even work?
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStarted() => Start();

    private void OnStopping() => Stop();

    private static void OnStopped() { }

    protected abstract void Start();

    protected abstract void Stop();
}