using AITUgameJam.scripts.plants;
using Godot;

namespace AITUgameJam.scripts.weaponary.bullets;

public partial class Bullet : Area2D
{
    [Export] public float Damage { get; set; }
    
    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Plant plant) return;
        plant.TakeDamage(Damage);
        QueueFree();
    }
    
}