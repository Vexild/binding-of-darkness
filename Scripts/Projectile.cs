using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export]
	public int Speed = 500;
	[Export]
	public float ProjectileRange = 600.0f;
	[Export]
	public int ProjectileDamage = 2;
	public Vector2 ProjectileDirection { get; set; }
	private bool ProjectileInMotion = true;
	private float DistanceTraveled;
	[Signal]
	public delegate void DeleteProjectileEventHandler();

	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("Animation").Play("greenprojectile");
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
				if (ProjectileDirection.X == -1)
				{
					if (ProjectileDirection.Y > 0)
					{
						// C# does not allow using Position.X += ... because 'Node2D.Position' is not a variable
						SetPosition(new Vector2(Position.X, (float)(Position.Y + SpeedDelta)));
					}
					else
					{
						SetPosition(new Vector2((float)(Position.X - SpeedDelta), Position.Y));
					}
				}
				else if (ProjectileDirection.X == 1 && ProjectileDirection.Y == 0)
				{
					if (ProjectileDirection.Y < 0)
					{
						SetPosition(new Vector2(Position.X, (float)(Position.Y - SpeedDelta)));
					}
					else
					{
						SetPosition(new Vector2((float)(Position.X + SpeedDelta), Position.Y));
					}
				}
				else if (ProjectileDirection.Y > 0)
				{
					SetPosition(new Vector2(Position.X, (float)(Position.Y + SpeedDelta)));
				}
				else if (ProjectileDirection.Y < 0)
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
