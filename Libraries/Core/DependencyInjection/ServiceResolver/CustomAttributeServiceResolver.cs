using System.Reflection;
using Core.DependencyInjection.Core.Attributes;

namespace Core.DependencyInjection.ServiceResolver;

public sealed class CustomAttributeServiceResolver : IServiceResolver
{
    private readonly ServiceResolversLoader _serviceResolversLoader;
    private readonly IDictionary<Type, IDictionary<MemberTypes, MemberInfo[]>> _cachedResolvers;

    private readonly ICollection<object> _resolvedMembersCollection;

    public CustomAttributeServiceResolver(ServiceResolversLoader serviceResolversLoader)
    {
        _serviceResolversLoader = serviceResolversLoader;
        _resolvedMembersCollection = new HashSet<object>();
        _cachedResolvers = new Dictionary<Type, IDictionary<MemberTypes, MemberInfo[]>>();
    }

    public void Resolve(object context)
    {
        if (context == null) 
            throw new ArgumentNullException(nameof(context));

        if (_resolvedMembersCollection.Contains(context))
            return;

        var type = context.GetType();
        if (!_cachedResolvers.TryGetValue(type, out var resolvers))
        {
            resolvers = context
                .GetType()
                .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(member => member.GetCustomAttribute(typeof(InjectAttribute), true) is not null)
                .Distinct()
                .GroupBy(member => member.MemberType)
                .ToDictionary(member => member.Key, member => member.ToArray());

            _cachedResolvers.Add(type, resolvers);
        }

        foreach (var member in resolvers)
        {
            _serviceResolversLoader
                .GetServiceResolver(member.Key)
                .ResolveMembers(context, member.Value);
        }

        _resolvedMembersCollection.Add(context);
    }
}