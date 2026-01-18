using Godot;
using System;
using System.Linq;
using AITUgameJam.scripts.plants;

namespace AITUgameJam.scripts.mobs;


public partial class AntUnit : Unit
{
    
    [Export] public float Acceleration = 600f;
    [Export] public float StopDistance = 4f;
    [Export] public float AttackInterval = 1f;
    [Export] public float AttackRange = 8f;

    private float _attackTimer = 0f;

    private Vector2 _velocity = Vector2.Zero;

    
    Vector2 GoTo = Vector2.Zero;
    private Plant _currentTarget;
    [Export] private Timer checkTimer;
    private bool check = false;

    public override void _Ready()
    {
        base._Ready();
        Search();
        checkTimer.Timeout += Search;
    }

    void Search()
    {
        
        var list = GetTree().GetFirstNodeInGroup("map").GetChildren().OfType<Plant>().ToList();
        var closest = list.OrderBy(p => p.GlobalPosition.DistanceSquaredTo(GlobalPosition))
            .FirstOrDefault();
        if (closest == null)
        {
            check = true;
            return;
        }
        check = false;
        _currentTarget = closest;
        GoTo = closest.Position;
        checkTimer.Stop();
        
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        if (!IsInstanceValid(_currentTarget))
        {
            _currentTarget = null;
            if (checkTimer.IsStopped()) checkTimer.Start();
        }
        if (_currentTarget != null)
        {
            MoveToTarget(dt);
             TryAttack(dt);
        }
    }
    
    void MoveToTarget(float delta)
    {
        Vector2 toTarget = GoTo - GlobalPosition;
        float distance = toTarget.Length();

        if (distance <= StopDistance) {
            _velocity = _velocity.MoveToward(Vector2.Zero, Acceleration * delta);
            Velocity = _velocity;
            MoveAndSlide();
            return;
        }

        Vector2 desiredVelocity = toTarget.Normalized() * MaxSpeed;

        _velocity = _velocity.MoveToward(
            desiredVelocity,
            Acceleration * delta
        );

        Velocity = _velocity;
        MoveAndSlide();
    }

    void TryAttack(float delta)
    {
        if (!IsInstanceValid(_currentTarget))
        {
            _currentTarget = null;
            if (checkTimer.IsStopped()) checkTimer.Start();
            check = false;
            return;
        }
        
        float distSq = _currentTarget.GlobalPosition.DistanceSquaredTo(GlobalPosition);
        if (distSq > AttackRange * AttackRange) {
            _attackTimer = 0f;
            return;
        }
        GD.Print("pul srenk");
        _attackTimer += delta;
        if (_attackTimer >= AttackInterval)
        {
            GD.Print("attacking");
            _attackTimer -= AttackInterval;
            _currentTarget.TakeDamage(Damage);
        }
    }

}