using Core.DependencyInjection.Container;
using Core.DependencyInjection.Core;
using Core.DependencyInjection.Exceptions;
using Core.DependencyInjection.Host.Extensions;
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
            .ConfigureServices(ConfigureServices)
            .Configure(Configure)
            .BuildAndRunConfigurableHostAsync(CancellationTokenSource.Token);

        if (Host is null)
            throw new DependencyInjectionException(
                "Error trying to build IHost. Maybe there is an error creating the IHostBuilder.");
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
        var hostedServices = app.ApplicationServices.GetServices<IHostedService>();
        var containerCollection = app.ApplicationServices.GetRequiredService<IContainerCollection>();

        if (startup is null)
            throw new DependencyInjectionException("IStartup object required!");

        if (hostedServices is null || !hostedServices.Any())
            throw new DependencyInjectionException("IHostedService object required!");

        startup.Configure(app, env);
        containerCollection.Configure(app, env);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            CancellationTokenSource.CancelAsync().Forget();
        } // default behavior
    }
}