namespace Core.DependencyInjection.Exceptions;

public sealed class DependencyInjectionException : Exception
{
    public DependencyInjectionException(string message) : base(message)
    {
    }

    public DependencyInjectionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}