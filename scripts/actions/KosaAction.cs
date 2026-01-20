using Godot;
using System;
using AITUgameJam.scripts.actions;

public partial class KosaAction : ActionNode
{
	private PackedScene ks = GD.Load<PackedScene>("res://scenes/items/kosilka.tscn");
	[Export] Timer timer;
	bool executed = false;
	public override void _Ready()
	{
		base._Ready();
		if (Item.Type == "kosa")
			Item.MainHandAction += Attacking;
		timer.Timeout += () =>
		{
			executed = false;
			timer.Stop();
		};
	}

	void Attacking()
	{
		if (executed) return;
		executed = true;
		var goat = ks.Instantiate<Node2D>();
		
		Tween tween = GetTree().CreateTween();
    
		tween.TweenProperty(Item, "rotation_degrees", 360f, 0.6f)
			.AsRelative()
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);
		
		goat.Rotation = GetAngleTo(GetGlobalMousePosition());
		goat.Position = 16 * Vector2.FromAngle(goat.Rotation);
		Item.GetParent().AddChild(goat);
		timer.Start();
	}
	
	public override void _ExitTree()
	{
		Item.MainHandAction -= Attacking;
	}
}
