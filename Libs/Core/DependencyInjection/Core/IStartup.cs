using Core.DependencyInjection.Container;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.DependencyInjection.Core;

public interface IStartup
{
    void ConfigureContainers(IContainerCollection containerCollection);

    void ConfigureServices(IServiceCollection serviceCollection);

    void Configure(IApplicationBuilder app, IHostEnvironment env);
}