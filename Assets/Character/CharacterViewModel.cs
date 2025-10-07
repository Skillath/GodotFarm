using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.MVVM;
using Core.Observable;
using Godot;
using JetBrains.Annotations;

namespace RealFriendlyFarm.Assets.Character;

[UsedImplicitly]
public sealed partial class CharacterViewModel : ViewModelBase
{
    public ObservableProperty<Vector3> Position { get; } = new();

    [ObservableProperty] 
    private bool _isFoo;

    [RelayCommand]
    private void ChangePosition(Vector3 position)
    {
        Position.Value += position;
        IsFoo = !IsFoo;
    }
}