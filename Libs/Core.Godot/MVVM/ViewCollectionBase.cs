using Core.MVVM;
using Godot;

namespace Core.Godot.MVVM;

public abstract class ViewCollectionBase : IViewCollection
{
    private readonly Dictionary<Type, PackedScene> _views = new();
    
    public PackedScene? GetView<TViewModel>() 
        where TViewModel : IViewModel
    {
        if (!_views.TryGetValue(typeof(TViewModel), out var packedScene))
        {
            return null;
        }

        return packedScene;
    }
}