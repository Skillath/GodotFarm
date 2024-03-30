namespace Core.Observable;

public delegate void RelayCommandCallback();
public sealed class RelayCommand
{
    private readonly RelayCommandCallback _command;

    public RelayCommand(RelayCommandCallback command)
    {
        _command = command;
    }

    public void Dispatch()
    {
        _command.Invoke();
    }
}

public delegate void RelayCommandCallback<in TParameter>(TParameter parameter);
public sealed class RelayCommand<TParameter>
{
    private readonly RelayCommandCallback<TParameter> _command;

    public RelayCommand(RelayCommandCallback<TParameter> command)
    {
        _command = command;
    }

    public void Dispatch(TParameter parameter)
    {
        _command.Invoke(parameter);
    }
}