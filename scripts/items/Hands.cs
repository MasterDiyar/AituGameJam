using Godot;
using System;
using AITUgameJam.scripts.mobs;
using AITUgameJam.scripts.weaponary.bullets;

public partial class Hands : AnimatedSprite2D
{
	private CatWarrior a;
	private float Damage;
	PackedScene scene = GD.Load<PackedScene>("res://scenes/items/arrow_line.tscn");
	public override void _Ready()
	{
		a = GetParent<CatWarrior>();
		a.Attack += Attack;
		AnimationFinished += Shoot;
	}

	private float shootAngle=0;
	void Attack(float angle, float damage)
	{
		shootAngle =angle;
		Damage = damage;
		Play();
		
	}

	private void Shoot()
	{
		var bar = scene.Instantiate<Node2D>();
		var gar = bar.GetNode<Bullet>("Arrow");
			gar.Rotation = shootAngle;
			gar.Damage = Damage;
		bar.GlobalPosition = GlobalPosition;
		GetTree().GetFirstNodeInGroup("map").AddChild(bar);
	}
}
