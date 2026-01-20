using System.Linq;

namespace AITUgameJam.scripts.mobs;

using System;
using Godot;
using AITUgameJam.scripts.interfaces;


public partial class CatWarrior : Kitten
{
    [Export] public float Acceleration = 600f;
    [Export] public float AttackInterval = 1f;
    [Export] public float AttackRange = 32f;

    private bool _isSelected = false;
    private Vector2 _goTo;
    private Vector2 _velocity;

    private float _attackTimer = 0f;
    private IGetHurt _currentEnemy;

    private Area2D _attackArea;

    public override void _Ready()
    {
        base._Ready();
        _attackArea = GetNode<Area2D>("AttackArea");
        _attackArea.BodyEntered += OnEnemyEntered;
        _attackArea.BodyExited += OnEnemyExited;
        sprite.Play();
        
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        if (_isSelected) {
            HandleMoveInput();
            MoveToPoint(dt);
        }
        HandleAttack(dt);
        if (Velocity != Vector2.Zero) sprite.Animation = "walk";
        else sprite.Animation = "idle";
    }

    #region Selection

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is not InputEventMouseButton { ButtonIndex: MouseButton.Left } mouseEvent) return;
        if (!mouseEvent.Pressed) return;
        _isSelected = true;
        GetViewport().SetInputAsHandled();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventMouseButton { ButtonIndex: MouseButton.Left } mouseEvent) return;
        if (!mouseEvent.Pressed) return;
        _isSelected = false;
    }
    
    

    #endregion

    #region Movement

    private void HandleMoveInput()
    {
        if (Input.IsActionJustPressed("rm"))
            _goTo = GetGlobalMousePosition();
        
    }

    private void MoveToPoint(float delta)
    {
        Vector2 toTarget = _goTo - GlobalPosition;
        float distanceSq = toTarget.LengthSquared();

        if (distanceSq < 2f) {
            GlobalPosition = _goTo; 
            _velocity = Vector2.Zero;
        } 
        else if (distanceSq < 100f){
            _velocity = _velocity.MoveToward(Vector2.Zero, Acceleration * 2 * delta);
        } else {
            Vector2 desiredVelocity = toTarget.Normalized() * MaxSpeed;
            _velocity = _velocity.MoveToward(desiredVelocity, Acceleration * delta);
        }

        Velocity = _velocity;
        MoveAndSlide();
    }

    #endregion

    #region Attack

    private void OnEnemyEntered(Node body)
    {
        if (body.IsInGroup("enemy") && body is IGetHurt hurt)
        {
            _currentEnemy = hurt;
            _attackTimer = 0f;
        }
    }

    private void OnEnemyExited(Node body)
    {
        if (body is IGetHurt hurt && hurt == _currentEnemy)
        {
            _currentEnemy = null;
            _attackTimer = 0f;
        }
    }

    public Action<float, float> Attack;
    private void HandleAttack(float delta)
    {
        if (_currentEnemy == null)
            return;
        _attackTimer += delta;

        if (_attackTimer >= AttackInterval) {
            _attackTimer -= AttackInterval;
            var pos = _currentEnemy as Node2D;
            var angle = GetAngleTo(pos.Position);
            Attack?.Invoke(angle, Damage);
        }
    }

    #endregion
}
