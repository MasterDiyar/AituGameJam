using Godot;
using System;
namespace AITUgameJam.scripts.mobs;

public partial class Koroleva : AntUnit
{
	public override void OnDeath()
	{
		AnimatedSprite.Play("Dead");
		AnimatedSprite.AnimationFinished += QueueFree;
	}
}
