namespace Core.DependencyInjection.Core;

public interface IApplicationBuilder
{
    IServiceProvider ApplicationServices { get; }
}