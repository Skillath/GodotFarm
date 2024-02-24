namespace Core.DependencyInjection;

public interface IDependencyResolver
{
    void ResolveDependencies(object instance);
}