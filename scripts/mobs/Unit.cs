using Godot;

namespace AITUgameJam.scripts.mobs;

public partial class Unit : CharacterBody2D
{
    [Export] public float MaxHp { get; set; } = 200;
    [Export] public float MaxSpeed { get; set; } = 200;
    
    float Hp = 0;
}