using System;
using AITUgameJam.scripts.mobs;
using AITUgameJam.scripts.playerThings;
using Godot;

namespace AITUgameJam.scripts.items;

public partial class Item : Sprite2D
{
    [Export] public int Count = 0;
    [Export] public string Type = "";
    [Export] public int itemID = 0;

    
    public Action MainHandAction, SecondHandAction;
    
    public override void _Ready()
    {
        if (GetParent() is Unit)
            OnHand();
    }

    public void OnHand()
    {
        GetParent<Unit>().leftClick += mhInvoker;
    }

    protected virtual void mhInvoker()
    {
        MainHandAction?.Invoke();
    }
    
    public int InventoryIndex;
    public Inventory MyInventory;

    protected void OnItemFinished()
    {
        MyInventory?.RemoveItem(InventoryIndex);
    }
}