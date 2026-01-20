using Godot;
using System;
using AITUgameJam.scripts.items;
using AITUgameJam.scripts.mobs;
using AITUgameJam.scripts.playerThings;

public partial class ThrowedItem : Area2D
{
	public Item item;

	public void Setup(PackedScene itemScene, int count)
	{
		item = itemScene.Instantiate<Item>();
		item.Count = count;
		AddChild(item);
	}
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;	
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Cat cat) {
			var inv = cat.GetNode<Inventory>("inventory");
			if (inv.AddItem(item))
				QueueFree();
		}
		else if (body is CatWarrior warrior)
		{
			switch (item.itemID)
			{
				case 3:
					warrior.Hp += 50;
					break;
				case 4:
					warrior.Damage += 100;
					break;
			}
			QueueFree();
		}
	}
}
