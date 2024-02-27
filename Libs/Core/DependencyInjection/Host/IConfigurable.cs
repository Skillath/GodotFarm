using Core.DependencyInjection.Core;
using Microsoft.Extensions.Hosting;

namespace Core.DependencyInjection.Host;

public interface IConfigurable
{
    IEnumerable<Action<IApplicationBuilder, IHostEnvironment>> ConfigureCollection { get; }

    void AddConfigure(Action<IApplicationBuilder, IHostEnvironment> configure);
}