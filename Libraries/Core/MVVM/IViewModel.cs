namespace Core.MVVM;

public interface IViewModel : IBindingContextHolder
{
    void Destroy();
}