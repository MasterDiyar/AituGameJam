using Godot;
using System;
using System.Linq;
using AITUgameJam.scripts.playerThings;

public partial class PlayerUi : CanvasLayer
{
	[Export] public Inventory playerInventory; 
	private InventorySlot[] uiSlots;
	[Export] HBoxContainer uiContainer, numsContainer;
	private Label[] uiLabels;
	[Export] Line2D uiLine;

	public override void _Ready()
	{
		uiSlots = uiContainer.GetChildren().OfType<InventorySlot>().ToArray();
		uiLabels = numsContainer.GetChildren().OfType<Label>().ToArray();
		for (int i = 0; i < playerInventory.items.Length; i++)
			if (playerInventory.items[i] != null)
				OnItemChanged(i, playerInventory.items[i]);

		playerInventory.ItemChanged += OnItemChanged;
		playerInventory.CurrentIndex += CurrentFrame;
	}

	void CurrentFrame(int frame)
	{
		uiLine.Position = Vector2.Right * frame * 100;
	}

	private void OnItemChanged(int index, PackedScene itemScene)
	{
		
		if (index >= 0 && index < uiSlots.Length)
		{
			var count = playerInventory.counter[index]; 
			uiSlots[index].SetItem(itemScene, uiLabels[index], count);
		}
		
	}
}
