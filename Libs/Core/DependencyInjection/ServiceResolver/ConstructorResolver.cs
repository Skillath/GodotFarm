using System.Reflection;
using Core.DependencyInjection.Exceptions;
using JetBrains.Annotations;

namespace Core.DependencyInjection.ServiceResolver;

[UsedImplicitly]
public sealed class ConstructorResolver : MemberResolverBase
{
    public ConstructorResolver(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override IEnumerable<object> ResolveMembers(object context, IEnumerable<MemberInfo> members)
    {
        var resolvedConstructors = members?.Cast<ConstructorInfo>()?
           .Where(constructorInfo => constructorInfo is not null)
           .SelectMany(constructorInfo =>
           {
               var objectList = constructorInfo.GetParameters()
                   .Select(parameter => ServiceProvider.GetService(parameter.ParameterType))
                   .Where(service => service is not null)
                   .ToArray();

               constructorInfo.Invoke(context, objectList);

               return objectList;
           })
           .ToArray();

        return resolvedConstructors
            ?? throw new DependencyInjectionException($"Failed trying to cast MemberInfo into ConstructorInfo");
    }
}