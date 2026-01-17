namespace AITUgameJam.scripts.resources;
using Godot;

[GlobalClass] 
public partial class ItemEntry : Resource
{
    [Export] public PackedScene Scene;
    [Export] public Vector2I Coords;
}