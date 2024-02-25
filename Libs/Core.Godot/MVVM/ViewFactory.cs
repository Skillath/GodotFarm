using Core.DependencyInjection.ServiceResolver;
using Core.MVVM;
using Godot;
using JetBrains.Annotations;

namespace Core.Godot.MVVM;

[UsedImplicitly]
public sealed class ViewFactory : IViewFactory
{
    private readonly IViewCollection _viewCollection;
    private readonly IServiceResolver _dependencyResolver;

    public ViewFactory(
        IViewCollection viewCollection,
        IServiceResolver dependencyResolver)
    {
        _viewCollection = viewCollection;
        _dependencyResolver = dependencyResolver;
    }

    public IView CreateView<TViewModel>()
        where TViewModel : IViewModel
    {
        var viewTemplate = _viewCollection.GetView<TViewModel>();
        if (viewTemplate is null)
            throw new Exception($"Couldn't find view for ViewModel {nameof(TViewModel)}");
        
        var view = viewTemplate.Instantiate<IView>();
        _dependencyResolver.Resolve(view);
        return view;
    }

    public void DestroyView(IView view)
    {
        var node = (Node)view;
        node.QueueFree();
    }
}