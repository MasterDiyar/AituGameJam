using System;
using AITUgameJam.scripts.interfaces;
using Godot;

namespace AITUgameJam.scripts.mobs;

public partial class Unit : CharacterBody2D, IGetHurt
{
    [Export] public float MaxHp { get; set; } = 200;
    [Export] public float MaxSpeed { get; set; } = 200;
    [Export] public float Damage { get; set; } = 10;
    
    public float Hp = 0;
    public Action leftClick;

    public override void _Ready()
    {
        Hp = MaxHp;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("lm"))
            leftClick?.Invoke();
    }
    
    public virtual void TakeDamage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0) 
            QueueFree();
    }
}