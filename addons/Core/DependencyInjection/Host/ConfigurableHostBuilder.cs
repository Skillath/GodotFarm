using Core.DependencyInjection.Core;
using Microsoft.Extensions.Hosting;

namespace Core.DependencyInjection.Host;

public sealed class ConfigurableHostBuilder : HostBuilder, IConfigurable
{
    private readonly HashSet<Action<IApplicationBuilder, IHostEnvironment>> _configureCollection = new();

    public IEnumerable<Action<IApplicationBuilder, IHostEnvironment>> ConfigureCollection => _configureCollection;

    public void AddConfigure(Action<IApplicationBuilder, IHostEnvironment> configure)
    {
        if (_configureCollection.Contains(configure))
            return;

        _configureCollection.Add(configure);
    }
}