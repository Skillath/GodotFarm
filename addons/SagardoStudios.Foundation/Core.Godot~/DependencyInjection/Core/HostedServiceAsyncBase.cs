using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace Core.Godot.DependencyInjection.Core;

[UsedImplicitly]
public abstract class HostedServiceAsyncBase : IHostedService
{
    public abstract Task StartAsync(CancellationToken cancellationToken);

    public abstract Task StopAsync(CancellationToken cancellationToken);
}