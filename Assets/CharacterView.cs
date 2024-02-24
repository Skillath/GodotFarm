using Core.Godot.MVVM;
using Core.MVVM;
using Core.Observable;
using Godot;
using JetBrains.Annotations;

namespace RealFriendlyFarm.Assets;

[UsedImplicitly]
public sealed class CharacterViewModel : IViewModel
{
	private RelayCommand? _relayCommand;
	private AsyncRelayCommand? _asyncRelayCommand;
	
	public ObservableProperty<int> Number { get; } = new();

	public RelayCommand DoSomethingCommand => _relayCommand ??= new(DoSomething);
	public AsyncRelayCommand DoSomethingAsyncCommand => _asyncRelayCommand ??= new(DoSomethingWithArgs);
	
	private void DoSomething()
	{
		GD.Print("DoSomething");
	}

	private async Task DoSomethingWithArgs(CancellationToken cancellationToken)
	{
		await Task.Delay(1000, cancellationToken);
		GD.Print("DoSomethingWithArgs");
	}
}

public sealed partial class CharacterView : SceneViewBase<CharacterViewModel>
{
	protected override void Bind()
	{
		ViewModel.Number.OnValueChanged
			.Register(OnNumberChanged);
	}

	protected override void BeforeDestroy()
	{
		ViewModel.Number.OnValueChanged
			.Unregister(OnNumberChanged);
	}

	private void OnNumberChanged(int value)
	{
		ViewModel.DoSomethingCommand.Dispatch();
	}
}