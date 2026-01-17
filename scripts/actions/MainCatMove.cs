using AITUgameJam.scripts.mobs;
using Godot;

namespace AITUgameJam.scripts.actions;

public partial class MainCatMove : Node2D
{
    private Unit myCat;
    private float Speed = 0;
    [Export] float Acceleration = 100;
    
    
    public override void _Ready()
    {
        myCat = GetParent<Unit>();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 move = Input.GetVector("a", "d", "w", "s");
        if (!move.Equals(Vector2.Zero))
            Speed -=(Speed > 0) ? Acceleration * (float)delta : 0;
        Speed +=(Speed < myCat.MaxSpeed) ? Acceleration * (float)delta : 0;
        myCat.Velocity = move.Normalized() * Speed;
        myCat.MoveAndSlide();
    }
}