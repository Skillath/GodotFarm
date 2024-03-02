using Core.DependencyInjection.Container;
using Core.DependencyInjection.Core;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Godot.DependencyInjection.Container;

public abstract partial class ResourceContainerBase : Resource, IContainer
{
    public abstract void ConfigureServices(IServiceCollection serviceCollection);
    public abstract void Configure(IApplicationBuilder app, IHostEnvironment env);
}