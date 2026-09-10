namespace Core.Observable;

public readonly struct Binding
{
    private readonly WeakReference<Action> _disposePattern;

    private Binding(WeakReference<Action> disposePattern)
    {
        _disposePattern = disposePattern;
    }

    public static Binding Create(Action disposePattern)
    {
        return new Binding(new WeakReference<Action>(disposePattern));
    }

    public void ClearBinding()
    {
        if (!_disposePattern.TryGetTarget(out var pattern))
            throw new Exception("Couldn't find WeakReference pattern!");
        
        pattern.Invoke();
    } 
}