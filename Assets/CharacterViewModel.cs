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
    private AsyncRelayCommand? _asyncRelayCommand;

    public ObservableProperty<Vector3> Position { get; } = new();

    public RelayCommand<Vector3> ChangePositionCommand => _relayCommand ??= new(DoSomething);
    public AsyncRelayCommand DoSomethingAsyncCommand => _asyncRelayCommand ??= new(DoSomethingWithArgs);

    public CharacterViewModel(ILogger<CharacterViewModel> logger)
    {
        _logger = logger;
    }

    private void DoSomething(Vector3 position)
    {
        _logger.LogInformation(nameof(DoSomething));

        Position.Value += position;
    }

    private async Task DoSomethingWithArgs(CancellationToken cancellationToken)
    {
        
    }
}