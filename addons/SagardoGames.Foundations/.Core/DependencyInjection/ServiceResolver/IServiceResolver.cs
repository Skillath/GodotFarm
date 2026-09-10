namespace Core.DependencyInjection.ServiceResolver;

public interface IServiceResolver
{
    void Resolve(object context);
}