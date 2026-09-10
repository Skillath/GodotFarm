using System;
using Core.DependencyInjection.ServiceResolver;
using Core.MVVM;
using Godot;
using JetBrains.Annotations;

namespace Core.Godot.MVVM;

[UsedImplicitly]
public sealed class ViewFactory : IViewFactory
{
    private readonly Node _root;
    private readonly IViewCollection _viewCollection;
    private readonly IServiceResolver _dependencyResolver;

    public ViewFactory(
        Node root,
        IViewCollection viewCollection,
        IServiceResolver dependencyResolver)
    {
        _root = root;
        _viewCollection = viewCollection;
        _dependencyResolver = dependencyResolver;
    }

    public IView CreateView<TViewModel>()
        where TViewModel : IViewModel
    {
        var viewTemplate = _viewCollection.GetView<TViewModel>();
        
        if (viewTemplate is null)
            throw new Exception($"Couldn't find view for ViewModel {nameof(TViewModel)}");

        var node = viewTemplate.InstantiateAndResolve(
            _dependencyResolver,
            _root);
        
        return (IView)node;
    }

    public TView CreateViewNew<TView>() 
        where TView : IView
    {
        throw new NotImplementedException();
    }

    public void DestroyView(IView view)
    {
        var node = (Node)view;
        node.QueueFree();
    }
}