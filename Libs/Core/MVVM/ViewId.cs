namespace Core.MVVM;

public readonly record struct ViewId(Guid Value)
{
    public static ViewId Create() => new ViewId(Guid.NewGuid());
}