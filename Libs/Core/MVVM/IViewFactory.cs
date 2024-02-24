namespace Core.MVVM;

public interface IViewFactory
{
    IView CreateView<TViewModel>() 
        where TViewModel : IViewModel;

    void DestroyView(IView view);
}