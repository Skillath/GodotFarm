namespace Core.Observable;

public sealed class AsyncRelayCommand
{
    private readonly Func<CancellationToken, System.Threading.Tasks.Task> _command;

    public AsyncRelayCommand(Func<CancellationToken, System.Threading.Tasks.Task> command)
    {
        _command = command;
    }

    public System.Threading.Tasks.Task Dispatch(CancellationToken cancellationToken)
    {
        return _command.Invoke(cancellationToken);
    }
    
    public System.Threading.Tasks.Task Dispatch()
    {
        return Dispatch(CancellationToken.None);
    }
}