using Core.DependencyInjection.Container;
using Core.DependencyInjection.Core;
using Core.MVVM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Godot.MVVM;

public sealed class MvvmContainer : IContainer
{
    public void Dispose()
    {
        // TODO release managed resources here
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IViewFactory, ViewFactory>();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    { 
        
    }
}