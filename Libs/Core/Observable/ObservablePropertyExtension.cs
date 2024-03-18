namespace Core.Observable;

public static class ObservablePropertyExtension
{
    public static void RegisterValueChanged<TValue>(
        this ObservableProperty<TValue> property,
        ObservableEventCallback<TValue> callback,
        bool invokeOnObserve = true)
    {
        property.OnValueChanged.Register(callback);
        if (!invokeOnObserve)
            return;
        callback.Invoke(property.Value);
    }
    
    public static void UnregisterValueChanged<TValue>(
        this ObservableProperty<TValue> property,
        ObservableEventCallback<TValue> callback)
    {
        property.OnValueChanged.Unregister(callback);
    }

    public static void UnregisterAll<TValue>(
        this ObservableProperty<TValue> property)
    {
        property.OnValueChanged.Clear();
    }
}