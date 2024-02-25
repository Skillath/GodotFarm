using Core.Godot.MVVM;
using Core.Observable;

namespace RealFriendlyFarm.Assets;

public sealed partial class CharacterView : SceneViewBase<CharacterViewModel>
{
    protected override void Bind()
    {
        ViewModel.Number.RegisterValueChanged(OnNumberChanged);
    }

    protected override void BeforeDestroy()
    {
        ViewModel.Number.UnregisterValueChanged(OnNumberChanged);
    }

    private void OnNumberChanged(int value)
    {
        ViewModel.DoSomethingCommand.Dispatch();
    }
}