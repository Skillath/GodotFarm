using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.DependencyInjection.Core.Attributes;
using Core.MVVM;
using Core.Observable;
using Godot;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace RealFriendlyFarm.Assets;

[UsedImplicitly]
public sealed partial class CharacterViewModel : ViewModelBase
{
    private readonly ILogger<CharacterViewModel> _logger;
    public ObservableProperty<Vector3> Position { get; } = new();

    public CharacterViewModel(ILogger<CharacterViewModel> logger)
    {
        _logger = logger;
    }

    [RelayCommand]
    private void ChangePosition(Vector3 position)
    {
        Position.Value += position;
    }
}