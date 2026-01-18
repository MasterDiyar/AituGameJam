using Godot;
using System;

public partial class Start : Control
{
	[Export] private Button start;
	public override void _Ready()
	{
		start.Pressed += () =>
		{
			var map = GD.Load<PackedScene>("res://scenes/maps/test_map.tscn").Instantiate();
			GetParent().AddChild(map);
			QueueFree();
		};
	}
}
