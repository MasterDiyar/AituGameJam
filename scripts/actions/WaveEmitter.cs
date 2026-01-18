using Godot;
using System;
using System.Collections.Generic;
using AITUgameJam.scripts.mobs;

public partial class WaveEmitter : Node2D
{
	[Export] private PackedScene[] mobs = [GD.Load<PackedScene>("res://scenes/mobs/ant.tscn")];
	[Export] private Timer timer;
	private Node _spawnTarget;
	private int Wave = 0;
	private readonly RandomNumberGenerator _rng = new();

	public override void _Ready()
	{
		_rng.Randomize();
		
		SpawnNode = GetTree().GetFirstNodeInGroup("map");
		_spawnTarget = SpawnNode;
		timer.Start();
		timer.Timeout += TimerOnTimeout;
	}

	private void TimerOnTimeout()
	{
		Wave++;
		if (Wave < 15)
			HandMade(Wave);
		else
		{
			timer.Stop();
		}
	}
	
	public struct MSI
	{
		public int SPI;   //Spawn point index
		public int MID;   // mob id
		public int Count; // hz
	}

	static MSI M(int a, int b, int c) => new() { SPI = a, MID = b, Count = c }; 

	[Export] private float SpawnRadius = 100;
	Vector2[] Align = [new Vector2(1500, -300), new Vector2(1500, 2300),  new Vector2(3000, 1300)];

	private List<MSI[]> _WaveData =
	[
		[M(1, 0, 2), M(2, 0, 2)],
		[M(1, 0, 3), M(1, 1, 1), M(2, 0, 3), M(3, 0, 1)]
	];

	private Node SpawnNode;
	void HandMade(int index)
	{
		if (_spawnTarget == null) return;

		foreach (var info in _WaveData[index])
		{
			Vector2 center = Align[info.SPI-1];
			PackedScene prefab = mobs[info.MID];

			for (int i = 0; i < info.Count; i++)
				SpawnMob(prefab, center);
		}
	}
	
	private void SpawnMob(PackedScene prefab, Vector2 center)
	{
		var unit = prefab.Instantiate<Node2D>();
        
		float angle = _rng.Randf() * Mathf.Tau; 
		float r = _rng.Randf() * SpawnRadius;
		unit.Position = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;

		_spawnTarget.AddChild(unit);
	}
}
