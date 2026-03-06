using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


public partial class Player : CharacterBody2D
{
	private Util util = new Util();
	private CoreManager CoreManagerNode;
	private int PlayerHitPointsHalf { get; set; }
	private float PlayerSpeed { get; set; }
	private float ProjectileCooldown { get; set; }
	private bool IsProjectileOnCooldown { get; set; }
	private ProgressBar ShootTimerBar;
	private Timer ShootTimer;
	[Signal]
	public delegate void ShootProjectileEventHandler(float pos, Vector2 dir);
	[Signal]
	public delegate void PlayerHitPointsChangedEventHandler(float newValue);
	private Vector2 LatestShootingDirection;
	private Vector2 EnemyTargetPoint;
	public override void _Ready()
	{
		GD.Print("Player Spawned");
		InitializeDebuggingNodes();
		InitiateCoreManager();
		ApplyInitialStatsFromManager();

		GD.Print("Player Max Hit Points: " + CoreManagerNode.GetPlayerMaxHitPoints + "\n"
			+ "Player Hit Points: " + util.FromHalfUnits(PlayerHitPointsHalf) + "\n"
			+ "Player speed: " + PlayerSpeed + "\n"
			);
		ShootTimerBar.Value = ShootTimerBar.MaxValue;
		EnemyTargetPoint = GetNode<Marker2D>("EnemyTargetPoint").Position;
	}

	public override void _Process(double delta)
	{
		Vector2 move_input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = move_input * PlayerSpeed;
		MoveAndSlide();
		HandleSpriteFlips();
		if (!IsProjectileOnCooldown)
		{
			ListenShootingActionAndFire();
		}

		if (ShootTimerBar.Value < ShootTimerBar.MaxValue)
		{
			ShootTimerBar.Value += 1;
		}
	}

	private void InitializeDebuggingNodes()
	{
		ShootTimerBar = GetNode<ProgressBar>("ShootTimerBar");
		ShootTimer = GetNode<Timer>("ShootTimer");
	}

	private void InitiateCoreManager()
	{
		CoreManagerNode = GetParent() as CoreManager;
		if (CoreManagerNode == null)
		{
			GD.PushError("Player: CoreManager parent is missing or wrong type.");
		}
	}

	private void ApplyInitialStatsFromManager()
	{
		if (CoreManagerNode == null)
		{
			return;
		}

		PlayerHitPointsHalf = util.ToHalfUnits(CoreManagerNode.GetPlayerHitPoints);
		ProjectileCooldown = CoreManagerNode.GetPlayerProjectileCooldown;
		IsProjectileOnCooldown = CoreManagerNode.GetIsProjectileOnCooldown;
		PlayerSpeed = CoreManagerNode.PlayerSpeed;
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
			IsProjectileOnCooldown = true;

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
			IsProjectileOnCooldown = true;

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
			IsProjectileOnCooldown = true;

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
			IsProjectileOnCooldown = true;
		}
		ShootTimer.Start(ProjectileCooldown);
		ShootTimerBar.Value = 0;

	}
	public void GotHitByEnemy(Projectile proj)
	{
		if (proj.IsThisEnemyShot)
		{
			GD.Print("Got hit by projectile with damage: " + proj.ProjectileDamage);
			var damageHalf = util.ToHalfUnits(proj.ProjectileDamage);
			PlayerHitPointsHalf = Mathf.Max(0, PlayerHitPointsHalf - damageHalf);
			EmitSignal(SignalName.PlayerHitPointsChanged, util.FromHalfUnits(PlayerHitPointsHalf));
		}
	}
	public void OnShooTimerTimeout()
	{
		IsProjectileOnCooldown = false;
	}
}
