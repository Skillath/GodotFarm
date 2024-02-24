namespace Core.DependencyInjection;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class InjectAttribute : Attribute
{
    
}