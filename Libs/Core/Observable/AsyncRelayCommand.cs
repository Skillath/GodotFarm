namespace Core.Observable;

public delegate Task AsyncRelayCommandCallback(CancellationToken cancellationToken);
public sealed class AsyncRelayCommand
{
    private readonly AsyncRelayCommandCallback _command;

    public AsyncRelayCommand(AsyncRelayCommandCallback command)
    {
        _command = command;
    }

    public Task Dispatch(CancellationToken cancellationToken)
    {
        return _command.Invoke(cancellationToken);
    }

    public void Dispatch()
    {
        Dispatch(CancellationToken.None).Forget();
    }
    
}

public delegate Task AsyncRelayCommandCallback<in TParameter>(TParameter parameter, CancellationToken cancellationToken);
public sealed class AsyncRelayCommand<TParameter>
{
    private readonly AsyncRelayCommandCallback<TParameter> _command;

    public AsyncRelayCommand(AsyncRelayCommandCallback<TParameter> command)
    {
        _command = command;
    }

    public Task Dispatch(TParameter parameter, CancellationToken cancellationToken)
    {
        return _command.Invoke(parameter, cancellationToken);
    }

    public void Dispatch(TParameter parameter)
    {
        Dispatch(parameter, CancellationToken.None).Forget();
    }
    
}