using Core.Observable;

namespace Core.MVVM;

public interface IBindingContextHolder
{
    BindingContext BindingContext { get; }
}