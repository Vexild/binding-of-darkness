using Godot;
using System;

public partial class ShooterEnemy : Enemy
{
	[Export]
	public PackedScene EnemyProjectileScene = GD.Load<PackedScene>("res://Scenes/EnemyProjectile.tscn");
	private bool EnemyReady = false;
	public bool GetEnemyReady { get { return EnemyReady; } }
	public void SetEnemyReady(bool value) { EnemyReady = value; }
	[Signal]
	public delegate void SpawnNewEnemyProjectileEventHandler(Vector2 position, PackedScene enemyProjectile, float damage);
	private Timer EnemyProjectileCooldown;
	public override void _Ready()
	{
		GD.Print("Shooter Enemy Spawned");
		InitiateCoreManager();
		EnemyProjectileCooldown = GetNode<Timer>("EnemyShootCooldown");
		EnemyProjectileCooldown.Start();
	}

	public override void _Process(double delta)
	{
		if (GetEnemyReady && CoreManagerNode.IsPlayerAlive())
		{
			ShootAtPlayer();
		}
	}

	private void ShootAtPlayer()
	{

		if (CoreManagerNode.IsPlayerAlive())
		{
			GD.Print("ShooterEnemy. Emiting signal to shoot");
			EmitSignal(SignalName.SpawnNewEnemyProjectile, Position, EnemyProjectileScene, util.NormalizeHalfStep(EnemyBaseDamage));
			EnemyProjectileCooldown.Start();
			SetEnemyReady(false);
		}
	}
	private void EnemyShootCooldownFinished()
	{
		SetEnemyReady(true);
	}
	private void EnemyReadyAtAction()
	{
		SetEnemyReady(true);
	}
}
