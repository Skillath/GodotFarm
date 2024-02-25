using Core.MVVM;
using Core.Observable;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace RealFriendlyFarm.Assets;

[UsedImplicitly]
public sealed class CharacterViewModel : IViewModel
{
    private readonly ILogger<CharacterViewModel> _logger;

    private RelayCommand? _relayCommand;
    private AsyncRelayCommand? _asyncRelayCommand;

    public ObservableProperty<int> Number { get; } = new();

    public RelayCommand DoSomethingCommand => _relayCommand ??= new(DoSomething);
    public AsyncRelayCommand DoSomethingAsyncCommand => _asyncRelayCommand ??= new(DoSomethingWithArgs);

    public CharacterViewModel(ILogger<CharacterViewModel> logger)
    {
        _logger = logger;
    }

    private void DoSomething()
    {
        _logger.LogInformation(nameof(DoSomething));
    }

    private async Task DoSomethingWithArgs(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Calling {Name}", nameof(DoSomethingWithArgs));
        await Task.Delay(1000, cancellationToken);
        _logger.LogInformation("Succeed! {Name}", nameof(DoSomethingWithArgs));
    }
}