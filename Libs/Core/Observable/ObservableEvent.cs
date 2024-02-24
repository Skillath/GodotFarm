namespace Core.Observable;

public sealed class ObservableEvent
{
    private event Action Callbacks = delegate { };
    
    public void Invoke()
    {
        Callbacks.Invoke();
    }

    public void Register(Action callback)
    {
        Callbacks += callback;
    }

    public void Unregister(Action callback)
    {
        Callbacks -= callback;
    }
}

public sealed class ObservableEvent<TType>
{
    private event Action<TType> Callbacks = delegate { };
    
    public void Invoke(TType value)
    {
        Callbacks.Invoke(value);
    }

    public void Register(Action<TType> callback)
    {
        Callbacks += callback;
    }

    public void Unregister(Action<TType> callback)
    {
        Callbacks -= callback;
    }
}