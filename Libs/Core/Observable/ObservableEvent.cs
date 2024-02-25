namespace Core.Observable;

public delegate void ObservableEventCallback();
public sealed class ObservableEvent
{
    private event ObservableEventCallback? Callbacks;
    
    public void Invoke()
    {
        Callbacks?.Invoke();
    }

    public void Register(ObservableEventCallback callback)
    {
        Callbacks += callback;
    }

    public void Unregister(ObservableEventCallback callback)
    {
        Callbacks -= callback;
    }
    
    public void Clear()
    {
        Callbacks = null;
    }
}

public delegate void ObservableEventCallback<in TType>(TType parameter);
public sealed class ObservableEvent<TType>
{
    private event ObservableEventCallback<TType>? Callbacks;
    
    public void Invoke(TType value)
    {
        Callbacks?.Invoke(value);
    }

    public void Register(ObservableEventCallback<TType> callback)
    {
        Callbacks += callback;
    }

    public void Unregister(ObservableEventCallback<TType> callback)
    {
        Callbacks -= callback;
    }

    public void Clear()
    {
        Callbacks = null;
    }
}