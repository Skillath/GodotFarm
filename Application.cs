using Core.MVVM;
using RealFriendlyFarm.Assets;

namespace RealFriendlyFarm;

public sealed class Application
{
    private readonly IViewFactory _viewFactory;

    public Application(IViewFactory viewFactory)
    {
        _viewFactory = viewFactory;

        Start();
    }

    private void Start()
    {
        var view = (CharacterView)_viewFactory.CreateView<CharacterViewModel>();
    }
}