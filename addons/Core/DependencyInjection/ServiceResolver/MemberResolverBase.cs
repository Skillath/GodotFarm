using System.Reflection;

namespace Core.DependencyInjection.ServiceResolver;

public abstract class MemberResolverBase : IMemberResolver
{
    protected IServiceProvider ServiceProvider { get; }

    protected MemberResolverBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public abstract IEnumerable<object> ResolveMembers(object context, IEnumerable<MemberInfo> members);
}