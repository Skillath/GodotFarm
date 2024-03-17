using Core.MVVM;
using Godot;

namespace Core.Godot.MVVM;

public abstract class ViewCollectionBase : IViewCollection
{
    private readonly Dictionary<Type, PackedScene> _views = new();

    public void RegisterView<TView, TViewModel>(PackedScene view) 
        where TView : Node, IView 
        where TViewModel : IViewModel
    {
        _views.Add(typeof(TViewModel), view);
    }

    public PackedScene GetView<TViewModel>()
        where TViewModel : IViewModel
    {
        if (!_views.TryGetValue(typeof(TViewModel), out var packedScene))
            throw new Exception($"View for {nameof(TViewModel)} not found");

        return packedScene;
    }
}