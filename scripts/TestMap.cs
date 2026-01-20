using Godot;
using System;
using AITUgameJam.scripts.mobs;

public partial class TestMap : Node2D
{
	[Export] private Cat kitty;
	bool added = false;
	public override void _Process(double delta)
	{
		if (!IsInstanceValid(kitty) && !added) {
			added = true;
			AddChild(GD.Load<PackedScene>("res://scenes/diePanel.tscn").Instantiate());
		}
		if (!added) return;
		var a = GetTree().GetFirstNodeInGroup("enemy");
		if (IsInstanceValid(a)) a.QueueFree();

	}
}
