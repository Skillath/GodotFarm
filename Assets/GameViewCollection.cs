using Core.Godot.MVVM;
using Godot;

namespace RealFriendlyFarm.Assets;

public sealed class GameViewCollection : ViewCollectionBase
{
    [Export] 
    private CharacterView _characterView = default!;
}