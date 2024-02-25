using Microsoft.Extensions.Hosting;

namespace Core.DependencyInjection.Core;

public interface IProgram
{
    IHostBuilder CreateHostBuilder();
}