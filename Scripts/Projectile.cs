using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export]
	public int Speed = 300;
	[Export]
	public float ProjectileRange = 400.0f;
	private float DistanceTraveled;
	[Signal]
	public delegate void DeleteProjectileEventHandler();
	public override void _Ready()
	{
		// Projectile is launched with it's spinning animation
		GetNode<AnimatedSprite2D>("Animation").Play("greenprojectile");
	}

	public override void _Process(double delta)
	{
		float SpeedDelta = Speed * (float)delta;
		// Once we reach max range for the projectile, animation ends
		if (DistanceTraveled >= ProjectileRange)
		{
			GetNode<AnimatedSprite2D>("Animation").Play("hit");
		}
		else
		{
			DistanceTraveled += SpeedDelta;
			// C# does not allow using Position.X += ... because 'Node2D.Position' is not a variable 
			SetPosition(new Vector2((float)(Position.X - SpeedDelta), Position.Y));
		}

	}
	public void _on_animation_animation_finished()
	{
		QueueFree();
	}
}
