using System.Reflection;
using Core.DependencyInjection.Exceptions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyInjection.ServiceResolver;

//TODO: Maybe we need to remove this one as it's not used.
[UsedImplicitly]
public sealed class ConstructorResolver : MemberResolverBase
{
    public ConstructorResolver(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override IEnumerable<object> ResolveMembers(object context, IEnumerable<MemberInfo> members)
    {
        var serviceProvider = ServiceProvider;
        var resolvedConstructors = members?
            .Cast<ConstructorInfo>()?
            .Where(constructorInfo => constructorInfo is not null)
            .SelectMany(constructorInfo =>
            {
               var objectList = constructorInfo
                   .GetParameters()
                   .Select(parameter =>
                   {
                       var keyedServiceAttribute = GetKey(parameter.Member);
                       return keyedServiceAttribute is null 
                           ? serviceProvider.GetService(parameter.ParameterType) 
                           : serviceProvider.GetRequiredKeyedService(parameter.ParameterType, keyedServiceAttribute) 
                           ?? throw new DependencyInjectionException($"Type {parameter.ParameterType} not registered in the Service Collection");
                   })
                   .Where(service => service is not null)
                   .Select(service => service!)
                   .ToArray();
               
               constructorInfo.Invoke(context, objectList);

               return objectList;
            })
            .ToArray();

        return resolvedConstructors
            ?? throw new DependencyInjectionException($"Failed trying to cast MemberInfo into ConstructorInfo");
    }
}