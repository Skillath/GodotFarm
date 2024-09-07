namespace Core.Observable;

public static class ObservablePropertyExtension
{
    public static Binding RegisterValueChanged<TValue>(
        this ObservableProperty<TValue> property,
        ObservableEventCallback<TValue> callback,
        bool invokeOnObserve = true)
    {
        var binding = property.ValueChanged.Register(callback);

        if (invokeOnObserve)
        {
            callback.Invoke(property.Value);
        }
            
        return binding;
    }
    
    public static void UnregisterValueChanged<TValue>(
        this ObservableProperty<TValue> property,
        ObservableEventCallback<TValue> callback)
    {
        property.ValueChanged.Unregister(callback);
    }

    public static void UnregisterAll<TValue>(
        this ObservableProperty<TValue> property)
    {
        property.ValueChanged.Clear();
    }
}

public readonly struct Binding
{
    private readonly WeakReference<Action> _disposePattern;

    public Binding(Action disposePattern)
    {
        _disposePattern = new WeakReference<Action>(disposePattern);
    }

    public void ClearBinding()
    {
        if (!_disposePattern.TryGetTarget(out var pattern))
            throw new Exception("Couldn't find WeakReference pattern!");
        
        pattern.Invoke();
    } 
}

public readonly struct BindingContext : IDisposable
{
    private readonly List<Binding> _bindings = new();

    public BindingContext()
    {
    }

    public void AddBinding(Binding binding)
    {
        _bindings.Add(binding);
    }

    public void ClearBindings()
    {
        foreach (var binding in _bindings)
        {
            binding.ClearBinding();
        }
        
        _bindings.Clear();
    }

    void IDisposable.Dispose()
    {
        ClearBindings();
    }
}