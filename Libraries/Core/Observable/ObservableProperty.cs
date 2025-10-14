using CommunityToolkit.Mvvm.ComponentModel;

namespace Core.Observable;

public sealed partial class ObservableProperty<TValue> : ObservableObject
{
    [ObservableProperty]
    private TValue _value;

    public ObservableEvent<TValue> ValueChanged { get; } = new();
    public ObservableEvent<TValue> ValueChanging { get; } = new();

    public ObservableProperty() : this(default!)
    {
    }

    public ObservableProperty(TValue value)
    {
        _value = value;
    }
    
    private void SetValueWithoutNotifying(in TValue value)
    {
#pragma warning disable MVVMTK0034
        _value = value;
#pragma warning restore MVVMTK0034
    }

    partial void OnValueChanged(TValue value)
    {
        ValueChanged.Invoke(value);
    }

    partial void OnValueChanging(TValue value)
    {
        ValueChanging.Invoke(value);
    }
}