using AITUgameJam.scripts.interfaces;
using AITUgameJam.scripts.items;
using AITUgameJam.scripts.resources;
using AITUgameJam.scripts.weaponary.bullets;
using Godot;

namespace AITUgameJam.scripts.plants;

public partial class Plant : Area2D, IGetHurt
{
    [Export] public float Hp = 100;
    [Export] public Timer GrowthTimer;
    [Export] public Texture2D[] GrowTexture;
    [Export] public Sprite2D GrowSprite;
    [Export] public Godot.Collections.Array<ItemEntry> InventoryItems;
    [Export] public float DieChance = 90;
    public int GrowIndex = 0;
    private float Moisture = 0;
    protected PackedScene abortion;
    
    public override void _Ready()
    {
        abortion = GD.Load<PackedScene>("res://scenes/items/throwed_item.tscn");
        GrowthTimer.Timeout += GrowUp;
        GrowthTimer.Start();
    }

    protected virtual void GrowUp()
    {
        GrowIndex++;
        if (GrowIndex >= GrowTexture.Length) {
            Grown();
            if (GD.RandRange(0, 100) > DieChance)
                GrowIndex = 0;
            else return;
        }
        GrowSprite.Texture = GrowTexture[GrowIndex];
    }

    public void SpeedUp()
    {
        GrowthTimer.WaitTime /= 2;
    }

    protected virtual void Grown()
    {
        GrowthTimer.Timeout -= GrowUp;
        GiveItem();
        QueueFree();
    }

    protected virtual void GiveItem()
    {
        foreach (var scene in InventoryItems)
        {
            var count = GD.RandRange(scene.Coords.X, scene.Coords.Y);
            if (count <= 0) return;
            var abort = abortion.Instantiate<ThrowedItem>();
            abort.Position = GlobalPosition + Vector2.FromAngle(GD.Randf() * 6.28f)*10;
            GetTree().GetFirstNodeInGroup("map").AddChild(abort);
            abort.Setup(scene.Scene, count);
        }
    }
    
    public void TakeDamage(float amount)
    {
        Hp -= amount;
        if (Hp <= 0)
            QueueFree();
    }
}