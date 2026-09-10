using Core.DependencyInjection.Core;
using Microsoft.Extensions.DependencyInjection;
using IHostEnvironment = Microsoft.Extensions.Hosting.IHostEnvironment;

namespace Core.DependencyInjection.Container;

public interface IContainerCollection : IDisposable
{
    IContainerCollection RegisterSubContainer(IContainer container);
    void ConfigureServices(IServiceCollection serviceCollection);
    void Configure(IApplicationBuilder app, IHostEnvironment env);
}