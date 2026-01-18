using Godot;
using System;

public partial class StaticInfo : Node
{
	public float Money = 15;

	public static readonly PackedScene[] StatScenes = [
		GD.Load<PackedScene>("res://scenes/plants/wheat.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/cactus.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/rise.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/romashka.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/shrooms.tscn"),
		GD.Load<PackedScene>("res://scenes/plants/cotton.tscn"),
	];
}
