namespace Core.DependencyInjection;

public sealed class DependencyResolver : IDependencyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public DependencyResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public void ResolveDependencies(object instance)
    {
        throw new NotImplementedException();
    }
}