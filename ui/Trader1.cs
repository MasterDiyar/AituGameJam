using Godot;
using System;

public partial class Trader1 : Sprite2D
{
	Area2D _area;
	Label _label;
	public override void _Ready()
	{
		_area = GetNode<Area2D>("Area2D");
		_label = GetNode<Label>("Label");
		_area.BodyEntered += AreaOnBodyEntered;
		_area.BodyExited += AreaOnBodyExered;
	}
	bool buyer = false;

	private void AreaOnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			_label.Visible = true;
			buyer = true;
		}
	}

	void AreaOnBodyExered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			_label.Visible = false;
			buyer = false;
		}
	}
	
	public override void _Process(double delta)
	{
		if (buyer && Input.IsActionPressed("e"))
		{
			var a = GetNode<StaticInfo>("/root/StaticInfo");
				if (a.Money > 10)
				{
					a.Money -= 10;
					var b = GD.Load<PackedScene>("res://scenes/mobs/cat_warrior.tscn").Instantiate<Node2D>();
					b.Position = Position + Vector2.Down * 20;
					GetParent().AddChild(b);
				}}
	}
}
