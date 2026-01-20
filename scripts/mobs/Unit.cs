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
    
    public virtual void TakeDamage(float damage)
    {
        Hp -= damage;
        Tween damageTween = GetTree().CreateTween();
        Modulate = new Color(1, 0, 0); 
    
        damageTween.TweenProperty(this, "modulate", new Color(1, 1, 1), 0.5f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);

        if (Hp <= 0) 
            OnDeath();
    }

    public virtual void OnDeath()
    {
        QueueFree();
    }
}