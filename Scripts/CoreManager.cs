using System;
using Godot;


public partial class CoreManager : Node2D
{
	[Export]
	public PackedScene ProjectileScene = GD.Load<PackedScene>("res://Scenes//Projectile.tscn");
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
	public Vector2 GetPlayerPosition { get { return PlayerPosition; } }
	private Util util;
	private Node2D EnemyShots;
	private Player Player;
	public override void _Ready()
	{
		util = new Util();
		InitializePlayer();
		InitializeNodes();

		// ** TEST AREA **
		// Instantiate here a map. Map scene should have a simple spawn point and should make lifring
		// for spawning enemies. When the enemies spanw, we can call connect to connect their signals

			// TODO: Get the RommNode and set it to be the TestRoom in the Scenes/Rooms/TestRoom.tscn. Then, we can spawn an enemy in the TestRoom and connect the signal of the enemy to the Core Manager. This way, we can test the shooting mechanism of the shooter enemy and player projectile without worrying about the spawn system of the enemies.
				// No, we need a mapManager for that. The map manager will be responsible for spawning the rooms and enemies. So, we can call the map manager to spawn the TestRoom and the ShooterEnemy in that room. Then, we can connect the signal of the ShooterEnemy to the Core Manager to test the shooting mechanism. This way, we can also test the spawn system of the enemies in the future when we have more enemy types and room types.
		// ** TEST AREA END **
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
	private void OnEnemyShootAtPlayer(Vector2 ShooterPosition, PackedScene enemyProjectile, float damage)
	{
		GD.Print("OenemyShootAtPlayer ACTION");
		damage = util.NormalizeHalfStep(damage);
		if (PlayerAlive)
		{
			Vector2 Direction = ShooterPosition.DirectionTo(PlayerPosition);
			Projectile newEnemyProjectile = enemyProjectile.Instantiate() as Projectile;

			// Set all attributes here. Later, we want to make a single shoot function
			// that takes in the nature of the shooter, distance, damage, speed, delays
			// projectile numbers and other attributes 
			newEnemyProjectile.IsThisEnemyShot = true;
			newEnemyProjectile.ProjectileDirection = Direction;
			newEnemyProjectile.ProjectileDamage = damage;
			EnemyShots.AddChild(newEnemyProjectile);
			newEnemyProjectile.Position = ShooterPosition;
		}
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
		Player.QueueFree();
	}

	private void InitializePlayer()
	{
		PlayerAlive = true;
		PlayerMaxHitPoints = util.NormalizeHalfStep(PlayerMaxHitPoints);
		PlayerHitPoints = util.NormalizeHalfStep(PlayerHitPoints);
		if (PlayerHitPoints > PlayerMaxHitPoints)
		{
			PlayerHitPoints = PlayerMaxHitPoints;
		}
	}
	private void InitializeNodes()
	{		
		EnemyShots = GetNode<Node2D>("EnemyShots");
		Player = GetNode<Player>("Player");
	}
}
