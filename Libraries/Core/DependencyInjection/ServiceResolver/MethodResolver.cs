using System.Reflection;
using Core.DependencyInjection.Exceptions;
using JetBrains.Annotations;

namespace Core.DependencyInjection.ServiceResolver;

[UsedImplicitly]
public sealed class MethodResolver : MemberResolverBase
{
    public MethodResolver(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override IEnumerable<object> ResolveMembers(object context, IEnumerable<MemberInfo> members)
    {
        var resolvedMembers = members?.Cast<MethodInfo>()?
            .Where(methodInfo => methodInfo is not null)
            .SelectMany(methodInfo =>
            {
                var objectList = methodInfo.GetParameters()
                    .Select(parameter => ServiceProvider.GetService(parameter.ParameterType))
                    .Where(service => service is not null)
                    .ToArray();

                methodInfo?.Invoke(context, objectList);
                return objectList;
            })
            .ToHashSet();

       return resolvedMembers 
              ?? throw new DependencyInjectionException($"Failed trying to cast MemberInfo into MethodInfo"); ;
    }
}