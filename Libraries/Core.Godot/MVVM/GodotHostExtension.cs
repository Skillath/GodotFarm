using Microsoft.Extensions.Hosting;

namespace Core.Godot.MVVM;

public static class GodotHostExtension
{
    public static IHostBuilder UseMvvm(this IHostBuilder hostBuilder)
    {
        return hostBuilder;
    }
}