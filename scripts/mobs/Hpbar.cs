using Godot;
using System;
using AITUgameJam.scripts.mobs;
using AITUgameJam.scripts.plants;

public partial class Hpbar : Label
{
	[Export]private Unit u;
	[Export] private Plant p;
	[Export] bool isPlant = false;
	public override void _Process(double delta)
	{
		Text = $"HP: {((!isPlant) ? u.Hp : p.Hp)}";
	}
}
