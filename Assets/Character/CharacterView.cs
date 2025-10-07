using Core.Godot.MVVM;
using Core.Observable;
using Godot;

namespace RealFriendlyFarm.Assets.Character;

public sealed partial class CharacterView : ViewBase<CharacterViewModel>
{
    [Export] 
    private Node3D _object = default!;

    protected override void Bind()
    {
        BindingContext.AddBinding(ViewModel.Position.RegisterValueChanged(OnPositionChanged));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        var inputDirection = Input.GetVector("left", "right", "up", "down");
        var velocity = new Vector3(inputDirection.X, inputDirection.Y, 0);

        if (velocity == Vector3.Zero)
            return;

        var position = velocity * (float)delta * 10;
        ViewModel.ChangePositionCommand.Execute(position);
    }

    private void OnPositionChanged(Vector3 parameter)
    {
        _object.Position = parameter;
    }
}