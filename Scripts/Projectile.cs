using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export]
	public double velocity = 90.0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GlobalPosition = spawnPosition;
		// GlobalRotation = spawnRotation;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
	}
}
