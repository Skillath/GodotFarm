namespace Core.Observable;

public sealed class ObservableProperty<TValue>
{
    private TValue _value;

    public TValue Value
    {
        get => _value;
        set => SetValue(ref value);
    }

    public ObservableEvent<TValue> OnValueChanged { get; } = new();

    public ObservableProperty() : this(default!)
    {
    }

    public ObservableProperty(TValue value)
    {
        _value = value;
    }

    private void SetValue(ref TValue value)
    {
        if (EqualityComparer<TValue>.Default.Equals(value, _value))
            return;
        
        _value = value;
        OnValueChanged.Invoke(value);
    }
    
    private void SetValueWithoutNotifying(ref TValue value)
    {
        _value = value;
    }
}