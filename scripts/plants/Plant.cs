using AITUgameJam.scripts.interfaces;
using AITUgameJam.scripts.weaponary.bullets;
using Godot;

namespace AITUgameJam.scripts.plants;

public partial class Plant : Area2D, IGetHurt
{
    [Export] public float Hp = 100;
    [Export] public Timer GrowthTimer;
    [Export] public Texture2D[] GrowTexture;
    [Export] public Sprite2D GrowSprite;
    protected int GrowIndex = 0;
    
    public override void _Ready()
    {
        GrowthTimer.Timeout += GrowUp;
    }

    protected virtual void GrowUp()
    {
        GrowIndex++;
        if (GrowIndex >= GrowTexture.Length)
            Grown();
        GrowSprite.Texture = GrowTexture[GrowIndex];
        
    }

    protected virtual void Grown()
    {
        GrowthTimer.Timeout -= GrowUp;
        GiveItem();
        QueueFree();
    }

    protected virtual void GiveItem()
    {
        
    }
    
    public void TakeDamage(float amount)
    {
        Hp -= amount;
        
        if (Hp <= 0)
            QueueFree();
    }
}