using System.Reflection;
using System.Runtime.CompilerServices;
using Core.DependencyInjection.Exceptions;
using JetBrains.Annotations;

namespace Core.DependencyInjection.ServiceResolver;

[UsedImplicitly]
public sealed class PropertyResolver : MemberResolverBase
{
    private const string BackingFieldName = "<{0}>k__BackingField";

    private readonly ServiceResolversLoader _serviceResolversLoader;

    private IMemberResolver? _resolver;

    public PropertyResolver(IServiceProvider serviceProvider, ServiceResolversLoader serviceResolversLoader) : base(serviceProvider)
    {
        _serviceResolversLoader = serviceResolversLoader;
    }

    public override IEnumerable<object> ResolveMembers(object context, IEnumerable<MemberInfo> members)
    {
        _resolver ??= _serviceResolversLoader.GetServiceResolver(MemberTypes.Field);

        var propertyInfos = members.Cast<PropertyInfo>()
            ?? throw new DependencyInjectionException($"Failed trying to cast MemberInfo into PropertyInfo");

        var fields = new List<FieldInfo>();
        foreach (var propertyInfo in propertyInfos)
        {
            var fieldInfo = GetBackingFieldFromPropertyInfo(propertyInfo)!;

            _ = ServiceProvider.GetService(propertyInfo.PropertyType)
                ?? throw new DependencyInjectionException($"Type {propertyInfo.PropertyType} not registered in the Service Collection");

            fields.Add(fieldInfo);
        }

        return _resolver.ResolveMembers(context, fields);
    }

    private static FieldInfo? GetBackingFieldFromPropertyInfo(PropertyInfo propertyInfo)
    {
        if (!propertyInfo.CanRead || !propertyInfo.GetGetMethod(nonPublic: true).IsDefined(typeof(CompilerGeneratedAttribute), inherit: true))
            return null;

        var backingField = propertyInfo.DeclaringType!.GetField(string.Format(BackingFieldName, propertyInfo.Name), BindingFlags.Instance | BindingFlags.NonPublic);

        if (backingField is null)
            return null;

        return !backingField.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true) ? null : backingField;
    }
}