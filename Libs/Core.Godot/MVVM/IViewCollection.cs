using Core.MVVM;
using Godot;

namespace Core.Godot.MVVM;

public interface IViewCollection
{
    PackedScene? GetView<TViewModel>() where TViewModel : IViewModel;
}