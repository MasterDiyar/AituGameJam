using Godot;
using System;
using System.Linq;
using AITUgameJam.scripts.mobs;
using AITUgameJam.scripts.playerThings;

public partial class PlayerUi : CanvasLayer
{
	[Export] public Inventory playerInventory; 
	private InventorySlot[] uiSlots;
	[Export] HBoxContainer _uiContainers, _numsContainers;
	private Label[] uiLabels;
	[Export] Line2D uiLine;
	[Export] TextureProgressBar uiProgressBar;
	[Export] private Label moneyCount;
	private Cat cattus;
	private StaticInfo st;

	public override void _Ready()
	{
		st = GetNode<StaticInfo>("/root/StaticInfo");
		cattus = GetParent<Cat>();
		uiSlots = _uiContainers.GetChildren().OfType<InventorySlot>().ToArray();
		uiLabels = _numsContainers.GetChildren().OfType<Label>().ToArray();
		for (int i = 0; i < playerInventory.items.Length; i++)
			if (playerInventory.items[i] != null)
				OnItemChanged(i, playerInventory.items[i]);

		playerInventory.ItemChanged += OnItemChanged;
		playerInventory.CurrentIndex += CurrentFrame;
		uiProgressBar.MaxValue = cattus.MaxHp;
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

	public override void _Process(double delta)
	{
		uiProgressBar.Value = cattus.Hp;
		moneyCount.Text = $": {st.Money}";
	}
}
