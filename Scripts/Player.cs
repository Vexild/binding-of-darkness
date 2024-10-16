using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int PlayerSpeed { get; set; } = 400;
	[Export]
	public PackedScene ProjectileScene { get; private set; }

	public Vector2 ScreenSize;


	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		GD.Print("Player Spawned");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 move_input = Input.GetVector("move_left", "move_right", "move_up", "move_down");

		Velocity = move_input * 600;
		MoveAndSlide();

		var animationSprite = GetNode<AnimatedSprite2D>("animation");
		// var velocity = Vector2.Zero;

		// if (Input.IsActionPressed("move_right")) {
		// 	velocity.X += 1;
		// }

		// if (Input.IsActionPressed("move_left")) {
		// 	velocity.X -= 1;
		// }

		// if (Input.IsActionPressed("move_up")) {
		// 	velocity.Y -= 1;
		// }

		// if (Input.IsActionPressed("move_down")) {
		// 	velocity.Y += 1;
		// }

		// if (velocity.Length() > 0) {
		// 	velocity = velocity.Normalized() * PlayerSpeed;
		// 	// play animation start
		// 	animationSprite.Play();
		// } else {
		// 	// play animation stop
		// 	animationSprite.Stop();
		// }

		// // Actual movement
		// Position += velocity * (float)delta;
		// Position = new Vector2(
		// 	x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
		// 	y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		// );

		// Sprite directions
		if (Velocity.Length() > 0) {
			animationSprite.Play();
		}
		else {
			animationSprite.Stop();
		}
		if (Velocity.X > 0)
		{
			// Diagonal walks have either up or down animation
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

		if (Input.IsActionPressed("shoot_left"))
		{
			GD.Print("Shoot left");

			// Set state is Shooting with direction parameter
		}

	}
}
