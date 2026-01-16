using System;

namespace AITUgameJam.scripts.items;
using Godot;
public partial class Item : Sprite2D
{
    [Export] public int Count = 0;

    
    Action MainHandAction, SecondHandAction;
    
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        
    }

    public override void _ExitTree()
    {
        
    }
}