using Core.MVVM;
using Godot;
using JetBrains.Annotations;

namespace Core.Godot.DependencyInjection.Container;

[UsedImplicitly]
public sealed partial class PackedView<TView> : PackedScene
    where TView : Node, IView
{
    private TView View => Instantiate<TView>();
    
    
    
}