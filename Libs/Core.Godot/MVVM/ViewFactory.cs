using Core.MVVM;
using Godot;

namespace Core.Godot.MVVM;

public sealed class ViewFactory : IViewFactory
{
    private readonly IViewCollection _viewCollection;
    private readonly IServiceProvider _serviceProvider;

    public ViewFactory(
        IViewCollection viewCollection,
        IServiceProvider serviceProvider)
    {
        _viewCollection = viewCollection;
        _serviceProvider = serviceProvider;
    }

    public IView CreateView<TViewModel>() 
        where TViewModel : IViewModel
    {
        throw new NotImplementedException();
    }

    public void DestroyView(IView view)
    {
        throw new NotImplementedException();
    }
}

public abstract class ViewCollectionBase : IViewCollection
{
    private readonly Dictionary<Type, PackedScene> _views = new();
    
    public PackedScene? GetView<TViewModel>() 
        where TViewModel : IViewModel
    {
        if (!_views.TryGetValue(typeof(TViewModel), out var packedScene))
            return null;

        return packedScene;
    }
}