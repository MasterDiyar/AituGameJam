using Godot;
using System;
using AITUgameJam.scripts.weaponary.bullets;

public partial class die : Panel
{
	[Export] private Button dies;
	public override void _Ready()
	{
		dies.Pressed += DiesOnPressed;
	}
	private void DiesOnPressed()
	{
		var lol = GD.Load<PackedScene>("res://scenes/start.tscn").Instantiate();
		GetTree().Root.AddChild(lol);
		GetParent().QueueFree();
	}
}
