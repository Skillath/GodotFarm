using Core.Observable;

namespace Core.MVVM;

public interface IView : IBindingContextHolder
{
    ViewId Id { get; }
}

public interface IBindingContextHolder
{
    BindingContext BindingContext { get; }
}