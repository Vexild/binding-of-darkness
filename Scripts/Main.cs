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

	public void OnPlayerShootProjectile(Godot.Vector2 playerPos, Godot.Vector2 direction) {
		// NOTE: Important to cast the instance as Projectile. Otherwise the instance would be a 'Node' without Position attribute
		var newProjectile = ProjectileScene.Instantiate() as Projectile;
		// add the direction to the instantiated projectile
		newProjectile.ProjectileDirection = direction;
		GetNode<Node2D>("Shots").AddChild(newProjectile);
		newProjectile.Position = playerPos;
		
	}
	public void OnDeleteProjectile() {
		QueueFree();
		GD.Print("Projectile removed");
	}
}
