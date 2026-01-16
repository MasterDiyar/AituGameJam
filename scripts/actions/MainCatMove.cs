using AITUgameJam.scripts.mobs;
using Godot;

namespace AITUgameJam.scripts.actions;

public partial class MainCatMove : Node2D
{
    private Unit myCat;
    private float Speed = 0;
    
    
    public override void _Ready()
    {
        myCat = GetParent<Unit>();
    }

    public override void _Process(double delta)
    {
        
    }
}