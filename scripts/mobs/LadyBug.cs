using Godot;
using System;
using System.Linq;
using AITUgameJam.scripts.mobs;

public partial class LadyBug : Unit
{
    [Export] public float Acceleration = 600f;
    [Export] public float StopDistance = 5f;
    [Export] public float AttackInterval = 0.8f;
    [Export] public float AttackRange = 10f;

    private float _attackTimer = 0f;
    private Vector2 _velocity = Vector2.Zero;
    private Cat _currentTarget;

    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] private Timer checkTimer;

    public override void _Ready()
    {
        base._Ready();
        SearchTarget();
        checkTimer.Timeout += SearchTarget;
        AnimatedSprite.Play();
    }

    void SearchTarget()
    {
        var cats = GetTree().GetNodesInGroup("cats").OfType<Cat>().ToList();

        var closest = cats
            .OrderBy(c => c.GlobalPosition.DistanceSquaredTo(GlobalPosition))
            .FirstOrDefault();

        if (closest != null)
        {
            _currentTarget = closest;
            checkTimer.Stop();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        if (!IsInstanceValid(_currentTarget))
        {
            _currentTarget = null;
            if (checkTimer.IsStopped()) checkTimer.Start();
            return;
        }

        MoveToTarget(dt);
        TryAttack(dt);
    }

    void MoveToTarget(float delta)
    {
        Vector2 toTarget = _currentTarget.GlobalPosition - GlobalPosition;
        float distance = toTarget.Length();

        if (distance <= StopDistance)
        {
            _velocity = _velocity.MoveToward(Vector2.Zero, Acceleration * delta);
        }
        else
        {
            Vector2 desiredVelocity = toTarget.Normalized() * MaxSpeed;
            _velocity = _velocity.MoveToward(desiredVelocity, Acceleration * delta);
        }

        Velocity = _velocity;
        MoveAndSlide();
    }

    void TryAttack(float delta)
    {
        float distSq = _currentTarget.GlobalPosition.DistanceSquaredTo(GlobalPosition);

        if (distSq <= AttackRange * AttackRange)
        {
            _attackTimer += delta;
            if (_attackTimer >= AttackInterval)
            {
                _attackTimer = 0;
                _currentTarget.TakeDamage(Damage);
            }
        }
        else
        {
            _attackTimer = 0;
        }
    }
}