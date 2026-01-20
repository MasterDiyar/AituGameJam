using AITUgameJam.scripts.items;
using AITUgameJam.scripts.mobs;
using Godot;

namespace AITUgameJam.scripts.playerThings;

public partial class Inventory : Node2D
{
    [Signal] public delegate void ItemChangedEventHandler(int index, PackedScene itemScene);
    [Signal] public delegate void CurrentIndexEventHandler(int index);
    public Item[] itemBuffer = new Item[5];
    public PackedScene[] items = new PackedScene[5];
    public int[] counter = new int[5]; 
    Item currentItem;
    int currentIndex = 0;

    private Unit Parent;

    public override void _Ready()
    {
        AddItem(GD.Load<PackedScene>("res://scenes/items/hoe.tscn").Instantiate<Item>());
        AddItem(GD.Load<PackedScene>("res://scenes/items/kosa.tscn").Instantiate<Item>());
        Parent = GetParent<Unit>();
        UpdateCurrentItem();
    }

    public override void _Input(InputEvent @event)
    {
        for (int i = 0; i < 5; i++) {
            string actionName = (i + 1).ToString();
            if (@event.IsActionPressed(actionName))
            {
                currentIndex = i;
                ChangeSlot(i);
            }
        }
        
        if (@event.IsActionPressed("mouse_up"))
        {
            int nextIndex = currentIndex - 1;
            if (nextIndex < 0) nextIndex = 4; 
            ChangeSlot(nextIndex);
        }
        else if (@event.IsActionPressed("mouse_down"))
        {
            int nextIndex = (currentIndex + 1) % 5; 
            ChangeSlot(nextIndex);
        }
    }

    private void ChangeSlot(int newIndex)
    {
        if (newIndex < 0 || newIndex >= items.Length) return;
        
        currentIndex = newIndex;
        EmitSignal(SignalName.CurrentIndex, currentIndex);
        UpdateCurrentItem();
    }

    public void EmitCurrent(int count)
    {
        counter[currentIndex] = count;
        EmitSignal(SignalName.ItemChanged, currentIndex, items[currentIndex]);
    }

    private void UpdateCurrentItem()
    {
        if (IsInstanceValid(currentItem)) 
            currentItem.QueueFree();

        if (items[currentIndex] == null) {
            currentItem = null;
            return;
        }

        currentItem = items[currentIndex].Instantiate<Item>();
        currentItem.Count = counter[currentIndex];
    
        currentItem.InventoryIndex = currentIndex;
        currentItem.MyInventory = this;

        GetParent().CallDeferred(Node.MethodName.AddChild, currentItem);
    }

    public bool AddItem(Item item)
    {
        var itemScene = GD.Load<PackedScene>(item.SceneFilePath);
        string newPath = itemScene.ResourcePath;

        for (int i = 0; i < items.Length; i++) {
            if (items[i] != null && items[i].ResourcePath == newPath) {
                counter[i] += item.Count;
                EmitSignal(SignalName.ItemChanged, i, items[i]);
                if (i == currentIndex) UpdateCurrentItem(); 
                return true;
            }
        }
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == null) {
                items[i] = itemScene;
                counter[i] = item.Count;
            
                EmitSignal(SignalName.ItemChanged, i, itemScene);
                if (i == currentIndex) UpdateCurrentItem();
                return true;
            }
        }
        return false;
    }
    
    public void RemoveItem(int index)
    {
        if (index < 0 || index >= items.Length) return;
    
        items[index] = null;
        counter[index] = 0;
        EmitSignal(SignalName.ItemChanged, index, (PackedScene)null);
    
        if (index == currentIndex)
        {
            UpdateCurrentItem();
        }
    }
}