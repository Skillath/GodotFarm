using Core.DependencyInjection.Core.Attributes;
using Core.MVVM;
using Godot;

namespace Core.Godot.MVVM;

public abstract partial class SceneViewBase<TViewModel> : Node3D, IView
    where TViewModel : IViewModel
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public ViewId Id { get; } = ViewId.Create();

    protected TViewModel ViewModel { get; private set; } = default!;

    protected CancellationToken CancellationToken => _cancellationTokenSource.Token;
    
    [Inject]
    protected void ConstructBase(TViewModel viewModel)
    {
        ViewModel = viewModel;
    }
    
    public override void _EnterTree()
    {
        base._EnterTree();

        Bind();
    }

    public override void _ExitTree()
    {
        BeforeDestroy();
        base._ExitTree();
        _cancellationTokenSource.Cancel();
    }

    protected abstract void Bind();

    protected abstract void BeforeDestroy();
}