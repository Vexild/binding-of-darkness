using Godot;
using System;
using System.Runtime.InteropServices;

public partial class Player : CharacterBody2D
{
	[Export]
	public int PlayerSpeed { get; set; } = 400;
	[Export]
	public float ProjectileSpeed { get; private set; } = 0.5f;

	public Boolean ProjectileCooldown { get; set; } = false;
	// Define the shooting signal with direction
	[Signal]
	public delegate void ShootProjectileEventHandler(float pos, Vector2 dir);

	public override void _Ready()
	{
		GD.Print("Player Spawned");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 move_input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Vector2 shoot_direction = Input.GetVector("shoot_left", "shoot_right", "shoot_up", "shoot_down");
		var animationSprite = GetNode<AnimatedSprite2D>("animation");
		Velocity = move_input * PlayerSpeed;
		MoveAndSlide();

		// Sprite directions
		// Diagonal walks have either up or down animation
		if (Velocity.Length() > 0)
		{
			animationSprite.Play();
		}
		else
		{
			animationSprite.Stop();
		}
		if (Velocity.X > 0)
		{
			if (Velocity.Y > 0)
			{
				animationSprite.Play("walk_down");
				animationSprite.FlipH = false;

			}
			else if (Velocity.Y < 0)
			{
				animationSprite.Animation = "walk_up";
				animationSprite.FlipH = false;
			}
			else
			{
				animationSprite.Animation = "walk_right";
				animationSprite.FlipV = false;
			}
		}
		else if (Velocity.X < 0)
		{
			if (Velocity.Y > 0)
			{
				animationSprite.Animation = "walk_down";
				animationSprite.FlipV = false;
			}
			else if (Velocity.Y < 0)
			{
				animationSprite.Animation = "walk_up";
				animationSprite.FlipH = false;
			}
			else
			{
				animationSprite.Animation = "walk_left";
				animationSprite.FlipV = false;
			}
		}
		else if (Velocity.Y < 0)
		{
			animationSprite.Animation = "walk_up";
			animationSprite.FlipH = false;
		}
		else if (Velocity.Y > 0)
		{
			animationSprite.Animation = "walk_down";
			animationSprite.FlipH = false;
		}

		// Shooting actions
		if ((shoot_direction.X != 0 || shoot_direction.Y != 0) && !ProjectileCooldown)
		{
			// Emmit Signal and capture it in Main
			EmitSignal(SignalName.ShootProjectile, Position, shoot_direction);
			ProjectileCooldown = true;
			GetNode<Timer>("ShootTimer").Start(ProjectileSpeed);
			GD.Print("SHOT");
		}
	}
	
	public void OnShooTimerTimeout()
	{
		ProjectileCooldown = false;
	}
}
