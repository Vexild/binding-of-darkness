using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public partial class Player : CharacterBody2D
{
	[Export]
	public int PlayerSpeed { get; set; } = 400;
	[Export]
	public float ProjectileSpeed { get; private set; } = 0.5f;

	public bool ProjectileCooldown { get; set; } = false;

	// Define the shooting signal with direction
	[Signal]
	public delegate void ShootProjectileEventHandler(float pos, Vector2 dir);
	private Vector2 LatestShootingDirection;
	public override void _Ready()
	{
		GD.Print("Player Spawned");
		// Debugging
		GetNode<ProgressBar>("ShootTimerBar").Value = GetNode<ProgressBar>("ShootTimerBar").MaxValue;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 move_input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = move_input * PlayerSpeed;
		MoveAndSlide();
		HandleSpriteFlips();
		if (!ProjectileCooldown)
		{
			ListenShootingActionAndFire();
		}

		// Debugging
		if (GetNode<ProgressBar>("ShootTimerBar").Value < GetNode<ProgressBar>("ShootTimerBar").MaxValue)
		{
			GetNode<ProgressBar>("ShootTimerBar").Value += 1;
		}

	}

	private void HandleSpriteFlips()
	{
		var animationSprite = GetNode<AnimatedSprite2D>("animation");
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
	}
	private void ListenShootingActionAndFire()
	{
		// TODO: Figure out better way
		if (Input.IsActionPressed("shoot_left"))
		{
			if (Input.IsActionPressed("shoot_up"))
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(0, -1));
			}
			else if (Input.IsActionPressed("shoot_down"))
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(0, 1));
			}
			else
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(-1, 0));
			}
			ProjectileCooldown = true;

		}
		else if (Input.IsActionPressed("shoot_right"))
		{
			if (Input.IsActionPressed("shoot_up"))
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(0, -1));
			}
			else if (Input.IsActionPressed("shoot_down"))
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(0, 1));
			}
			else
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(1, 0));
			}
			ProjectileCooldown = true;

		}
		else if (Input.IsActionPressed("shoot_up"))
		{
			if (Input.IsActionPressed("shoot_down"))
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(0, 1));
			}
			else
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(0, -1));
			}
			ProjectileCooldown = true;

		}
		else if (Input.IsActionPressed("shoot_down"))
		{
			if (Input.IsActionPressed("shoot_up"))
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(0, -1));
			}
			else
			{
				EmitSignal(SignalName.ShootProjectile, Position, new Vector2(0, 1));
			}
			ProjectileCooldown = true;
		}
		GetNode<Timer>("ShootTimer").Start(ProjectileSpeed);
		GetNode<ProgressBar>("ShootTimerBar").Value = 0;

	}
	public void OnShooTimerTimeout()
	{
		ProjectileCooldown = false;
	}
}
