using Godot;
using System;
namespace AITUgameJam.scripts.mobs;
public partial class Cat : Kitten
{
	public override void _Ready()
	{
		base._Ready();
		sprite.Play("idle");
	}

    public bool UiBlocked { get; set; } = false;

    

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("lm"))
			leftClick?.Invoke();
	
		AnimationWork();
	}

    void AnimationWork()
    {
        if (UiBlocked) {
            sprite.Animation = "idle";
            return;
        }

        if (Input.GetVector("w", "a", "s", "d") != Vector2.Zero)
            sprite.Animation = "walk";
        else
            sprite.Animation = "idle";
    }
}
