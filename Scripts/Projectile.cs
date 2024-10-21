using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export]
	public int Speed = 500;
	[Export]
	public float ProjectileRange = 400.0f;
	[Export]
	public int ProjectileDamage = 2;
	public Vector2 ProjectileDirection { get; set; }
	private bool ProjectileInMotion = true;
	private float DistanceTraveled;
	[Signal]
	public delegate void DeleteProjectileEventHandler();

	public override void _Ready()
	{
		// Projectile is launched with it's spinning animation
		GetNode<AnimatedSprite2D>("Animation").Play("greenprojectile");

		// Check direction
		GD.Print("My direction is: ", ProjectileDirection);
	}

	public override void _Process(double delta)
	{
		float SpeedDelta = Speed * (float)delta;
		// Once we reach max range for the projectile, animation ends
		if (DistanceTraveled >= ProjectileRange)
		{
			// Stop the motion
			ProjectileInMotion = false;
			GetNode<AnimatedSprite2D>("Animation").Play("hit");
		}
		else
		{
			DistanceTraveled += SpeedDelta;
			// Exloding shot should not move
			if (ProjectileInMotion)
			{	
				// We only want a direction of up down left and right. 
				// Find a way to convert this to one of these cardinal directions. 
				// Rightr now pressing in two diagonal directions causes a stationary projectile to spawn.
				if (ProjectileDirection.X == -1 && ProjectileDirection.Y == 0)
				{
					// C# does not allow using Position.X += ... because 'Node2D.Position' is not a variable
					SetPosition(new Vector2((float)(Position.X - SpeedDelta), Position.Y));
				}
				if (ProjectileDirection.X == 1 && ProjectileDirection.Y == 0)
				{
					SetPosition(new Vector2((float)(Position.X + SpeedDelta), Position.Y));
				}
				if (ProjectileDirection.X == 0 && ProjectileDirection.Y == 1)
				{
					SetPosition(new Vector2(Position.X, (float)(Position.Y + SpeedDelta)));
				}
				if (ProjectileDirection.X == 0 && ProjectileDirection.Y == -1)
				{
					SetPosition(new Vector2(Position.X, (float)(Position.Y - SpeedDelta)));
				}


			}
		}

	}
	public void _on_animation_animation_finished()
	{
		QueueFree();
	}

	public void OnEnemyContanct(Node2D node)
	{
		if (node.IsInGroup("enemy_dummy"))
		{
			ProjectileInMotion = false;
			GetNode<AnimatedSprite2D>("Animation").Play("hit");
		}
	}
}
