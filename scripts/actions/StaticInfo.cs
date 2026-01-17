using Godot;
using System;

public partial class StaticInfo : Node
{
	public float Money = 15;

	public static readonly PackedScene[] StatScenes = [
		GD.Load<PackedScene>("res://scenes/plants/wheat.tscn")
	];
}
