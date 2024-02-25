using Core.DependencyInjection.Container;
using Core.DependencyInjection.Core;
using Core.DependencyInjection.Exceptions;
using Core.DependencyInjection.Host.Extensions;
using Core.Godot.Logging;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Godot.DependencyInjection.Core;

public abstract partial class ProgramBase : Node, IProgram
{
    protected IHost? Host { get; private set; }
    
    protected CancellationTokenSource CancellationTokenSource { get; } = new();
    
    public override void _EnterTree()
    {
        base._EnterTree();
        
        Host = CreateHostBuilder()
            //.ConfigureServices(s => s.AddSingleton<IFactory<GameObject>, GameObjectFactory>())
            .ConfigureServices(ConfigureServices)
            .Configure(Configure)
            .ConfigureLogging(l => l.AddGodotLoggers())
            .BuildAndRunConfigurableHostAsync(CancellationTokenSource.Token);

        if (Host is null)
            throw new DependencyInjectionException($"Error trying to build IHost. Maybe there is an error creating the IHostBuilder.");
    }

    public override void _ExitTree()
    {
        CancellationTokenSource.Cancel(false);
        base._ExitTree();
    }

    public abstract IHostBuilder CreateHostBuilder();

    protected virtual void BeforeDestroy()
    {
        
    }

    protected virtual void OnHostCreated()
    {
        
    }
    
    private static void ConfigureServices(IServiceCollection serviceProvider)
    {
            
    }

    private static void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        var startup = app.ApplicationServices.GetRequiredService<IStartup>();
        var hostedService = app.ApplicationServices.GetRequiredService<IEnumerable<IHostedService>>();
        var containerCollection = app.ApplicationServices.GetRequiredService<IContainerCollection>();

        if (startup is null)
            throw new DependencyInjectionException("IStartup object required!");

        if (hostedService is null || !hostedService.Any())
            throw new DependencyInjectionException("IHostedService object required!");

        startup.Configure(app, env);
        containerCollection.Configure(app, env);
    }
}