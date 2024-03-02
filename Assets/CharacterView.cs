using Core.Godot.MVVM;
using Core.Observable;
using Godot;

namespace RealFriendlyFarm.Assets;

public sealed partial class CharacterView : SceneViewBase<CharacterViewModel>
{
    protected override void Bind()
    {
        ViewModel.Position.RegisterValueChanged(OnPositionChanged);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        var velocity = new Vector3(
                Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
                0f,
                Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward"))
            .LimitLength();
        
        if(velocity == Vector3.Zero)
            return;

        ViewModel.ChangePositionCommand.Dispatch(velocity * (float)delta);
    }

    private void OnPositionChanged(Vector3 parameter)
    {
        Position = parameter * (float)GetProcessDeltaTime();
        GD.Print("Position: " + Position);
    }

    protected override void BeforeDestroy()
    {
        ViewModel.Position.UnregisterValueChanged(OnPositionChanged);
    }
}