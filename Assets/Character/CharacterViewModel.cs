using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.MVVM;
using Godot;
using JetBrains.Annotations;

namespace RealFriendlyFarm.Assets.Character;

[UsedImplicitly]
public sealed partial class CharacterViewModel : ViewModelBase
{
    [ObservableProperty] 
    private Vector3 _position;

    [RelayCommand]
    private void ChangePosition(Vector3 position)
    {
        Position += position;
    }
}