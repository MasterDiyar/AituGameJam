using Godot;
using System;
namespace AITUgameJam.scripts.mobs;
public partial class Cat : Unit
{
	[Export] private AnimatedSprite2D sprite;
	public override void _Ready()
	{
		base._Ready();
		sprite.Play("idle");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		AnimationWork();
	}

	void AnimationWork()
	{
		if (Input.GetVector("w", "a", "s", "d") != Vector2.Zero)
			sprite.Animation = "walk";
		else
			sprite.Animation = "idle";
		
	}
}
