using Core.DependencyInjection.Host.Extensions;
using Core.Godot.DependencyInjection.Core;
using Microsoft.Extensions.Hosting;

namespace RealFriendlyFarm;

public sealed partial class Program : ProgramBase
{
    public override IHostBuilder CreateHostBuilder()
    {
        return GodotHost
            .CreateDefaultBuilder()
            .UseHostedService<GameHostedService>()
            .UseStartup<Startup>();
    }
}