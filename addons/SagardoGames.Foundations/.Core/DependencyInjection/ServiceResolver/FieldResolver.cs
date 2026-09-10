using System.Reflection;
using Core.DependencyInjection.Exceptions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyInjection.ServiceResolver;

[UsedImplicitly]
public sealed class FieldResolver : MemberResolverBase
{
    public FieldResolver(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override IEnumerable<object> ResolveMembers(object context, IEnumerable<MemberInfo> members)
    {
        var fieldInfos = members.Cast<FieldInfo>() 
            ?? throw new DependencyInjectionException($"Failed trying to cast MemberInfo into FieldInfo");

        var serviceProvider = ServiceProvider;
        var list = new HashSet<object>();
        foreach (var fieldInfo in fieldInfos)
        {
            var key = GetKey(fieldInfo);
            var value = (key is null 
                    ? ServiceProvider.GetService(fieldInfo.FieldType) 
                    : ServiceProvider.GetRequiredKeyedService(fieldInfo.FieldType, key))
                ?? throw new DependencyInjectionException($"Type {fieldInfo.FieldType} not registered in the Service Collection");

            fieldInfo.SetValue(context, value);
            list.Add(value);
        }

        return list;
    }
}