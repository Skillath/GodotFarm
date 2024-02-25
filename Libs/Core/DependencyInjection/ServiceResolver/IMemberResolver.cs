using System.Reflection;

namespace Core.DependencyInjection.ServiceResolver;

public interface IMemberResolver
{
    IEnumerable<object> ResolveMembers(object context, IEnumerable<MemberInfo> members);
}