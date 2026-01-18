using Godot;
using System;
using AITUgameJam.scripts.plants;

public partial class Cotton : Plant
{
	protected override void GiveItem()
	{
		foreach (var scene in InventoryItems)
		{
			var count = GD.RandRange(scene.Coords.X, scene.Coords.Y*5);
			if (count <= scene.Coords.Y*5-3) return;
			count = scene.Coords.Y;
			var abort = abortion.Instantiate<ThrowedItem>();
			abort.Position = GlobalPosition + Vector2.FromAngle(GD.Randf() * 6.28f)*10;
			GetTree().GetFirstNodeInGroup("map").AddChild(abort);
			abort.Setup(scene.Scene, count);
		}
	}
}
