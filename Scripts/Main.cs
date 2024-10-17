using Godot;
using System;
using System.Numerics;


public partial class Main : Node2D
{
	[Export]
	public PackedScene ProjectileScene = GD.Load<PackedScene>("res://Scenes//Projectile.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPlayerShootProjectile(Godot.Vector2 playerPos) {
		// NOTE: Important to cast the instance as Node2D. Otherwise the instance would not have a Position attribute
		var newProjectile = ProjectileScene.Instantiate() as Node2D;
		GetNode<Node2D>("Shots").AddChild(newProjectile);
		newProjectile.Position = playerPos;
		
	}
	public void OnDeleteProjectile() {
		QueueFree();
		GD.Print("Projectile removed");
	}
}
