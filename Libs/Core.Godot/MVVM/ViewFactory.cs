using Core.MVVM;
using Core.Observable;
using Godot;

namespace Core.Godot.MVVM;

public sealed class ViewFactory : IViewFactory
{
    private readonly IViewCollection _viewCollection;
    private readonly IDependencyResolver _dependencyResolver;

    public ViewFactory(
        IViewCollection viewCollection,
        IDependencyResolver dependencyResolver)
    {
        _viewCollection = viewCollection;
        _dependencyResolver = dependencyResolver;
    }

    public IView CreateView<TViewModel>() 
        where TViewModel : IViewModel
    {
        var viewTemplate = _viewCollection.GetView<TViewModel>();
        if (viewTemplate is null)
            throw new Exception();

        var view = viewTemplate.Instantiate<IView>();
        _dependencyResolver.ResolveDependencies(view);
        return view;

    }

    public void DestroyView(IView view)
    {
        var node = (Node)view;
        node.Free();
    }
}