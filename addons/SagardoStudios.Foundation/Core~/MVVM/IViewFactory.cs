namespace Core.MVVM;

public interface IViewFactory
{
    IView CreateView<TViewModel>() 
        where TViewModel : IViewModel;

    TView CreateViewNew<TView>() 
        where TView : IView;

    void DestroyView(IView view);
}