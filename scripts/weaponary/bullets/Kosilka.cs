using Godot;
using System;
using AITUgameJam.scripts.interfaces;
using AITUgameJam.scripts.mobs;
using AITUgameJam.scripts.plants;

public partial class Kosilka : Area2D
{
	[Export] public float Damage { get; set; }
	[Export] public float Speed { get; set; }
    AnimatedSprite2D sprite;
	public override void _Ready()
	{
		BodyEntered += OnAreaEntered;
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play();
		sprite.AnimationFinished += QueueFree;
	}

	private void OnAreaEntered(Node2D area)
	{
		switch (area)
		{
			case Plant or Kitten:
				return;
			case IGetHurt hh:
				hh.TakeDamage(Damage);
				break;
		}
	}

	
}
