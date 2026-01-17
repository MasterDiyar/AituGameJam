using System;
using Godot;

namespace AITUgameJam.scripts.mobs;

public partial class Unit : CharacterBody2D
{
    [Export] public float MaxHp { get; set; } = 200;
    [Export] public float MaxSpeed { get; set; } = 200;
    
    float Hp = 0;
    public Action leftClick;

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("lm"))
            leftClick?.Invoke();
    }
}