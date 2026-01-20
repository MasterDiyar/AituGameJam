using Godot;
using System;
using System.Collections.Generic;
using AITUgameJam.scripts.mobs;

public partial class WaveEmitter : Node2D
{
	[Export] private PackedScene[] mobs = [GD.Load<PackedScene>("res://scenes/mobs/worker.tscn"),
		GD.Load<PackedScene>("res://scenes/mobs/infant.tscn"),
		GD.Load<PackedScene>("res://scenes/mobs/koroleva.tscn"),
		GD.Load<PackedScene>("res://scenes/mobs/ladybug.tscn")
	];
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
		if (Wave < _WaveData.Count)
			HandMade(Wave);
		else
			AutoWaves(Wave);
		
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
		[M(1, 0, 1), M(2, 0, 1)],
		[M(1, 0, 2), M(1, 1, 1), M(2, 0, 2), M(3, 0, 1)],
		[M(1, 0, 3), M(1, 1, 1), M(2, 0, 3), M(3, 0, 1)],
		[M(1, 1, 2), M(1, 2, 1), M(2, 3, 1)],
		[M(1, 0, 5), M(1, 1, 1), M(2, 2, 1), M(3, 3, 2)],
		[M(1, 0, 6), M(1, 1, 3), M(2, 0, 3), M(3, 0, 3)],
		[M(1, 0, 6), M(1, 1, 3), M(2, 3, 3), M(3, 1, 3)],
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

	void AutoWaves(int wave)
	{
		if (_spawnTarget == null) return;
		int difficultyIncrease = (wave - 7) / 3;
    
		int baseGroupsCount = 3; 
		int totalGroups = baseGroupsCount + difficultyIncrease;

		Random random = new Random();

		for (int i = 0; i < totalGroups; i++)
		{
			int randomSPI = random.Next(1, Align.Length + 1); 
			int randomMID = random.Next(0, mobs.Length);      
        
			int baseCount = random.Next(2, 5); 
			int finalCount = baseCount + (wave / 4); 

			Vector2 center = Align[randomSPI - 1];
			PackedScene prefab = mobs[randomMID];

			for (int j = 0; j < finalCount; j++)
			{
				SpawnMob(prefab, center);
			}
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
