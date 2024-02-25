using Core.DependencyInjection;
using Core.DependencyInjection.Core.Attributes;
using Core.MVVM;
using Godot;

namespace Core.Godot.MVVM;

public abstract partial class GuiViewBase<TViewModel> : Control, IView
    where TViewModel : IViewModel
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    public ViewId Id { get; } = ViewId.New();

    protected TViewModel ViewModel { get; private set; } = default!;

    protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

    [Inject]
    private void ConstructBase(TViewModel viewModel)
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
        
        _cancellationTokenSource.Cancel();
        
        base._ExitTree();
    }

    protected abstract void Bind();

    protected abstract void BeforeDestroy();
}
