using Core.MVVM;
using Core.Observable;
using Godot;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace RealFriendlyFarm.Assets;

[UsedImplicitly]
public sealed class CharacterViewModel : IViewModel
{
    private readonly ILogger<CharacterViewModel> _logger;

    private RelayCommand<Vector3>? _relayCommand;
    public ObservableProperty<Vector3> Position { get; } = new();

    public RelayCommand<Vector3> ChangePositionCommand => _relayCommand ??= new(ChangePosition);

    public CharacterViewModel(ILogger<CharacterViewModel> logger)
    {
        _logger = logger;
    }

    private void ChangePosition(Vector3 position)
    {
        _logger.LogInformation(nameof(ChangePosition));

        Position.Value += position;
    }
}