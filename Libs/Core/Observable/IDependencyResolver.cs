namespace Core.Observable;

public interface IDependencyResolver
{
    void ResolveDependencies(object instance);
}