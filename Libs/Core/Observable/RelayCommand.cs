namespace Core.Observable;

public sealed class RelayCommand
{
    private readonly Action _command;

    public RelayCommand(Action command)
    {
        _command = command;
    }

    public void Dispatch()
    {
        _command.Invoke();
    }
}