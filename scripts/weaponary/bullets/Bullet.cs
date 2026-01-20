using AITUgameJam.scripts.interfaces;
using AITUgameJam.scripts.mobs;
using AITUgameJam.scripts.plants;
using Godot;

namespace AITUgameJam.scripts.weaponary.bullets;

public partial class Bullet : Area2D
{
    [Export] public float Damage { get; set; }
    [Export] public float Speed { get; set; }
    Line2D _line;
    
    public override void _Ready()
    {
        _line = GetParent().GetNode<Line2D>("Line2D");
        BodyEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Node2D area)
    {
        switch (area)
        {
            case Plant or Kitten:
                return;
            case IGetHurt hh:
                hh.TakeDamage(Damage);
                GetParent().QueueFree();
                break;
        }
    }

    public override void _Process(double delta)
    {
        Position += Vector2.FromAngle(Rotation) * Speed * (float)delta;
        _line.AddPoint(Position);
        if (_line.GetPointCount() > 20)
            _line.RemovePoint(0);
    }
}