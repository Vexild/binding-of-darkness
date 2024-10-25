using Godot;
using System;

public partial class Projectile : Area2D
{
	public bool IsThisEnemyShot = false;
	[Export]
	public int ProjectileSpeed = 500;
	[Export]
	public float ProjectileRange = 600.0f;
	[Export]
	public int ProjectileDamage = 2;
	public Vector2 ProjectileDirection { get; set; }
	public float ProjectileAngle { get; set; }
	private bool ProjectileInMotion = true;
	private float DistanceTraveled;
	[Signal]
	public delegate void DeleteProjectileEventHandler();

	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("Animation").Play("idle");
		if (IsThisEnemyShot)
		{
			GetNode<AnimatedSprite2D>("Animation").Rotate(ProjectileDirection.Angle());
		}
	}

	public override void _Process(double delta)
	{
		float SpeedDelta = ProjectileSpeed * (float)delta;
		// Once we reach max range for the projectile, animation ends
		if (!IsThisEnemyShot)
		{

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
		else
		{
			if (ProjectileInMotion)
			{
				var pos = Position + ProjectileSpeed * ProjectileDirection * (float)delta;
				pos.Rotated(ProjectileDirection.Angle());
				SetPosition(pos);
			}

		}
	}
	public void OnHitAnimationFinished()
	{
		QueueFree();
	}

	public void OnEnemyContanct(Node2D node)
	{
		if (!IsThisEnemyShot && node.IsInGroup("enemy_dummy"))
		{
			ProjectileInMotion = false;
			GetNode<AnimatedSprite2D>("Animation").Play("hit");
		}
	}

	public void OnPlayerContact(Node2D node)
	{
		if (IsThisEnemyShot && node.IsInGroup("player"))
		{
			ProjectileInMotion = false;
			GetNode<AnimatedSprite2D>("Animation").Play("hit");
		}
	}
}
