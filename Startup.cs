using Core.Godot.MVVM;
using Core.MVVM;
using Core.Task;
using Godot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RealFriendlyFarm;

public sealed partial class Startup : Node
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    public override void _Ready()
    {
        base._Ready();

        var host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(ConfigureServices)
            .ConfigureLogging(ConfigureLogging)
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureHostConfiguration(ConfigureHost)
            .Build();

        _ = host.Services.GetRequiredService<Application>();
        
        host.RunAsync(_cancellationTokenSource.Token)
            .Forget();
        
    }

    private void ConfigureHost(IConfigurationBuilder configuratioNBuilder)
    {
        
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _cancellationTokenSource.Cancel(false);
    }

    private void ConfigureAppConfiguration(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        
    }

    private void ConfigureLogging(HostBuilderContext hostBuilderContext, ILoggingBuilder loggingBuilder)
    {
        //TODO: Create logger for Godot :)    
    }
    
    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<Application>();
        services.AddSingleton<IViewFactory, ViewFactory>();
    }
}