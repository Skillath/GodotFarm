namespace Core.MVVM;

public interface IView : IBindingContextHolder
{
    ViewId Id { get; }
}