using Godot;
using System;
using AITUgameJam.scripts.items;
using AITUgameJam.scripts.mobs;
using AITUgameJam.scripts.playerThings;
using AITUgameJam.scripts.shop;

public partial class HBoxContainers : HBoxContainer
{
	private Inventory inv;
	Cat Player;
	StaticInfo sinfo;
	public override void _Ready()
	{
		Player = GetTree().GetFirstNodeInGroup("player") as Cat;
		inv = GetTree().GetFirstNodeInGroup("inventory") as Inventory;
		sinfo = GetNode<StaticInfo>("/root/StaticInfo");
		foreach (var c in GetChildren())
		{
			if (c.GetChildCount() > 0 && c.GetChild(1) is SeedButton s) s.Pressed += () => press(c, s);
		}
		
		Close();
		
	}

	void press(Node c, SeedButton script)
	{
		var ca = c.GetNode<Label>("MarginContainer/VBoxContainer/wheat").Text;
		int index = -1;
		switch (ca.ToLower()) // ToLower, чтобы не бояться больших букв
		{
			case "wheat": index = 0; break;
			case "cactus": index = 1; break;
			case "rice": index = 2; break; 
			case "chamomile": index = 3; break;
			case "shrooms": index = 4; break;
			case "cotton": index = 5; break;
		}
		if (index != -1)
		{
			if (script.IsSell)
			{
				if (FindInventoryIndex(StaticInfo.StatSeedScenes[index]) != -1)
				{
					var ass = FindInventoryIndex(StaticInfo.StatSeedScenes[index]);
					if (ass < 0) return;

					int count = inv.counter[ass];
					if (count <= 0) return;

					sinfo.Money += count * int.Parse(c.GetNode<Label>("MarginContainer/VBoxContainer/price").Text.Trim().Split(' ')[0]);

					inv.counter[ass] = 0;
					inv.RemoveItem(ass);
				}
			}else{
				var item = StaticInfo.StatSeedScenes[index].Instantiate<Item>();
				item.Count = 1;
				if (inv.AddItem(item))
					sinfo.Money -= int.Parse(c.GetNode<Label>("MarginContainer/VBoxContainer/price").Text.Trim().Split(' ')[0]);
			}
		}
	}
	
	private int FindInventoryIndex(PackedScene scene)
	{
		if (scene == null) return -1;
		string targetPath = scene.ResourcePath;

		for (int i = 0; i < inv.items.Length; i++)
		{
			if (inv.items[i] != null && inv.items[i].ResourcePath == targetPath)
				return i;
		}
		return -1;
	}

	public void Open()
	{
		Visible = true;
	}

	public void Close()
	{
		Visible = false;
	}

	public void _on_button_pressed()
	{
		Close();
	}
}
