namespace Core.Observable;

public delegate Task AsyncRelayCommandCallback(CancellationToken cancellationToken);

[Obsolete($"Use {nameof(CommunityToolkit.Mvvm.Input.AsyncRelayCommand)}", true)]
public sealed class AsyncRelayCommand
{
    private readonly AsyncRelayCommandCallback _command;

    public AsyncRelayCommand(AsyncRelayCommandCallback command)
    {
        _command = command;
    }

    public Task DispatchAsync(CancellationToken cancellationToken)
    {
        return _command.Invoke(cancellationToken);
    }

    public void Dispatch()
    {
        DispatchAsync(CancellationToken.None).Forget();
    }
}

public delegate Task AsyncRelayCommandCallback<in TParameter>(TParameter parameter, CancellationToken cancellationToken);

[Obsolete($"Use {nameof(CommunityToolkit.Mvvm.Input.AsyncRelayCommand)}", true)]
public sealed class AsyncRelayCommand<TParameter>
{
    private readonly AsyncRelayCommandCallback<TParameter> _command;

    public AsyncRelayCommand(AsyncRelayCommandCallback<TParameter> command)
    {
        _command = command;
    }

    public Task DispatchAsync(TParameter parameter, CancellationToken cancellationToken)
    {
        return _command.Invoke(parameter, cancellationToken);
    }

    public void Dispatch(TParameter parameter)
    {
        DispatchAsync(parameter, CancellationToken.None).Forget();
    }
}