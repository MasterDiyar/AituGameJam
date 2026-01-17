using AITUgameJam.scripts.items;
using Godot;

namespace AITUgameJam.scripts.playerThings;

public partial class InventorySlot : TextureRect
{
    public void SetItem(PackedScene itemScene, Label label, int count)
    {
        if (itemScene == null) {
            Texture = null;
            return;
        }

        var tempItem = itemScene.Instantiate<Item>();
        Texture = tempItem.Texture; 
        label.Text = count == 0 ? "" : count.ToString();
        tempItem.QueueFree();
    }
}