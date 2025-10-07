using Core.Godot.DependencyInjection.Container;
using Core.MVVM;
using Godot;

namespace Core.Godot.MVVM;

public interface IViewCollection
{
    public void RegisterView<TView, TViewModel>(PackedScene view)
        where TView : Node, IView
        where TViewModel : IViewModel;
    
    public void RegisterView<TView, TViewModel>(PackedView<TView> view)
        where TView : Node, IView
        where TViewModel : IViewModel;
    
    PackedScene GetView<TViewModel>() 
        where TViewModel : IViewModel;
}