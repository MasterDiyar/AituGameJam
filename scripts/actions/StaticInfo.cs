using Godot;
using System;

public partial class StaticInfo : Node
{
	public float Money = 25;

	public static readonly PackedScene[] StatScenes = [
		GD.Load<PackedScene>("res://scenes/plants/wheat.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/cactus.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/rise.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/romashka.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/shrooms.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/cotton.tscn"),
	];
	
	public static readonly PackedScene[] StatSeedScenes = [
		GD.Load<PackedScene>("res://scenes/items/wheat_seed.tscn"),
		GD.Load<PackedScene>("res://scenes/items/cactusseed.tscn"),
		GD.Load<PackedScene>("res://scenes/items/riseseed.tscn"),
		GD.Load<PackedScene>("res://scenes/items/romashkaseed.tscn"),
		GD.Load<PackedScene>("res://scenes/items/shroomseed.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/cotton_seed.tscn"),
	];
}
