using System;
using Godot;


public partial class CoreManager : Node2D
{
	[Export]
	public PackedScene ProjectileScene = GD.Load<PackedScene>("res://Scenes//Projectile.tscn");
	[Export]
	public PackedScene EnemyProjectileScene = GD.Load<PackedScene>("res://Scenes/EnemyProjectile.tscn");
	[Signal]
	public delegate void ShootEnemyProjectileEventHandler(float pos, Vector2 dir);
	[Signal]
	public delegate void UpdatePlayerHitPointsInUIEventHandler(float newValue);
	[Signal]
	public delegate void PlayerEliminatedEventHandler();
	[Export]
	public float PlayerSpeed { get; set; } = 400.0f;
	public float GetPlayerSpeed { get { return PlayerSpeed; } }
	public void SetPlayerSpeed(float value) { PlayerSpeed = value; }
	[Export]
	private float PlayerMaxHitPoints = 4.0f;
	public float GetPlayerMaxHitPoints { get { return PlayerMaxHitPoints; } }
	public void SetPlayerMaxHitPoints(float value) { PlayerMaxHitPoints = util.NormalizeHalfStep(value); }
	[Export]
	private float PlayerHitPoints = 4.0f;
	public float GetPlayerHitPoints { get { return PlayerHitPoints; } }
	public void SetPlayerHitPoints(float value) { PlayerHitPoints = util.NormalizeHalfStep(value); }
	[Export]
	private float ProjectileCooldown = 0.5f;
	public float GetPlayerProjectileCooldown { get { return ProjectileCooldown; } }
	public void SetPlayerProjectileCooldown(float value) { ProjectileCooldown = value; }
	[Export]
	private float ProjectileDamage = 1.0f;
	public float GetPlayerProjectileDamage { get { return ProjectileDamage; } }
	public void SetPlayerProjectileDamage(float value) { ProjectileDamage = util.NormalizeHalfStep(value); }

	private bool IsProjectileOnCooldown = false;
	public bool GetIsProjectileOnCooldown { get { return IsProjectileOnCooldown; } }
	public void SetIsProjectileOnCooldown(bool value) { IsProjectileOnCooldown = value; }
	private bool PlayerAlive;
	public bool IsPlayerAlive() { return PlayerAlive; }

	private Vector2 PlayerPosition;
	private Util util;

	public override void _Ready()
	{
		util = new Util();
		PlayerAlive = true;
		PlayerMaxHitPoints = util.NormalizeHalfStep(PlayerMaxHitPoints);
		PlayerHitPoints = util.NormalizeHalfStep(PlayerHitPoints);
		if (PlayerHitPoints > PlayerMaxHitPoints)
		{
			PlayerHitPoints = PlayerMaxHitPoints;
		}
	}
	public override void _Process(double delta)
	{
		if (PlayerAlive)
		{
			PlayerPosition = GetNode<CharacterBody2D>("Player") != null ? GetNode<CharacterBody2D>("Player").Position : Vector2.Zero;
		}
	}
	public void OnPlayerShootProjectile(Vector2 playerPos, Vector2 direction)
	{
		// NOTE: Important to cast the instance as Projectile. Otherwise the instance would be a 'Node' without Position attribute
		var newProjectile = ProjectileScene.Instantiate() as Projectile;
		// add the direction to the instantiated projectile
		newProjectile.ProjectileDirection = direction;
		GetNode<Node2D>("Shots").AddChild(newProjectile);
		newProjectile.ProjectileDamage = GetPlayerProjectileDamage;
		newProjectile.Position = playerPos;
	}
	private void OnEnemyShootAtPlayer(Vector2 ShooterPosition, float damage)
	{
		damage = util.NormalizeHalfStep(damage);
		if (PlayerAlive)
		{
			var Direction = ShooterPosition.DirectionTo(PlayerPosition);
			var newEnemyProjectile = EnemyProjectileScene.Instantiate() as Projectile;

			// Set all attributes here. Later, we want to make a single shoot function
			// that takes in the nature of the shooter, distance, damage, speed, delays
			// projectile numbers and other attributes 
			newEnemyProjectile.IsThisEnemyShot = true;
			newEnemyProjectile.ProjectileDirection = Direction;
			newEnemyProjectile.ProjectileDamage = damage;
			GetNode<Node2D>("EnemyShots").AddChild(newEnemyProjectile);
			newEnemyProjectile.Position = ShooterPosition;
		}

	}
	public void OnDeleteProjectile()
	{
		QueueFree();
	}
	public void OnPlayerHitPointsChanged(float newValue)
	{
		newValue = util.NormalizeHalfStep(newValue);
		GD.Print("Player Hit Points reduced to: " + newValue + "/" + PlayerMaxHitPoints);
		SetPlayerHitPoints(newValue);
		if (PlayerHitPoints <= 0)
		{
			OnPlayerEliminate();
			// TODO: emmit GAME OVER here
		}
		EmitSignal(SignalName.UpdatePlayerHitPointsInUI, newValue);

	}
	private void OnPlayerEliminate()
	{
		GD.Print("Player Eliminated");
		PlayerAlive = false;
		EmitSignal(SignalName.PlayerEliminated);
		GetNode<CharacterBody2D>("Player").QueueFree();
	}
}
