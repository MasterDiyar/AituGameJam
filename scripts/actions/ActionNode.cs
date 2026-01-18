using AITUgameJam.scripts.items;
using AITUgameJam.scripts.mapEditor;
using AITUgameJam.scripts.plants;
using AITUgameJam.scripts.playerThings;
using Godot;

namespace AITUgameJam.scripts.actions;

public partial class ActionNode : Node2D
{
    [Export] private int id = 0;
    protected Item Item;
    public override void _Ready()
    {
        Item = GetParent<Item>();
        
        if (Item.Type =="seed")
            Item.MainHandAction += SetSeed;
    }

    void SetSeed()
    {
        var someScene = StaticInfo.StatScenes[id];
        Item.Count--;
        var plant = someScene.Instantiate<Plant>();
        plant.Position = new Vector2(Mathf.Floor(GlobalPosition.X / 32)*32+16, Mathf.Floor(GlobalPosition.Y / 32)*32+16);
        var map = GetTree().GetFirstNodeInGroup("map");
        
        if (!map.GetNode<MossDrawer>("moss").HasMoss(plant.Position)) return;
        
        map.AddChild(plant);
        
        if (Item.Count <= 0)
        {
            var inv = Item.GetParent().GetNode<Inventory>("inventory");
            inv.RemoveItem(Item.InventoryIndex);
        }
    }

    public override void _ExitTree()
    {
        Item.MainHandAction -= SetSeed;
    }
    
}