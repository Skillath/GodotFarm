using JetBrains.Annotations;

namespace Core.DependencyInjection.Core.Attributes;

[MeansImplicitUse(ImplicitUseKindFlags.Access)]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, 
    AllowMultiple = true, 
    Inherited = true)]
public sealed class InjectAttribute : Attribute
{
    public object? Key { get; }
    
    public InjectAttribute(object? key = null)
    {
        Key = key;
    }
}