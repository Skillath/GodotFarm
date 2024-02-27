namespace Core.DependencyInjection.Core;

public class ApplicationBuilder : IApplicationBuilder
{
    public IServiceProvider ApplicationServices { get; }

    public ApplicationBuilder(IServiceProvider applicationServices)
    {
        ApplicationServices = applicationServices;
    }
}