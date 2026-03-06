using Godot;
using System;
using System.Security.Cryptography;

public partial class Enemy : Area2D
{

	[Export]
	public int EnemyMaxHP = 10;
	[Export]
	public float EnemyBaseDamage = 1.0f;
	[Signal]
	public delegate void OnHitByPlayerEventHandler();

	[Signal]
	public delegate void SpawnNewEnemyProjectileEventHandler(Vector2 position, float damage);

	private ProgressBar HitPointBar { get; set; }
	private float CurrentHp;
	private Timer EnemyProjectileCooldown;
	private bool EnemyReady = false;
	private AnimatedSprite2D enemyAnimation { get; set; }
	private CoreManager CoreManagerNode;
	private Vector2 PlayerLocation;
	private Util util;

	public override void _Ready()
	{
		InitiateCoreManager();
		
		EnemyProjectileCooldown = GetNode<Timer>("EnemyShootCooldown");
		EnemyProjectileCooldown.Start();
		util = new Util();
		CurrentHp = EnemyMaxHP;
		HitPointBar = GetNode<ProgressBar>("EnemyHP");
		enemyAnimation = GetNode<AnimatedSprite2D>("EnemyAnimation");
		HitPointBar.Value = EnemyMaxHP;
		enemyAnimation.Play("idle");
	}

	public override void _Process(double delta)
	{
		if (EnemyReady && PlayerIsAlive())
		{
			ShootAtPlayer();
		}
	}

	public void DecreaseHP(float loss)
	{
		loss = util.NormalizeHalfStep(loss);
		GD.Print("Enemy hit! Lost " + loss + " HP.");
		CurrentHp -= loss;
		if (CurrentHp <= 0)
		{
			GetNode<ProgressBar>("EnemyHP").Value = CurrentHp;
			enemyAnimation.Stop();
			enemyAnimation.Play("death_animation");
		}
		else
		{
			GetNode<ProgressBar>("EnemyHP").Value = CurrentHp;
		}
	}
	public void OnAreaEntered(Node2D proj)
	{
		if (proj.IsInGroup("PlayerProjectile"))
		{
			Projectile projectileCast = (Projectile)proj;
			float playerDamage = util.NormalizeHalfStep(projectileCast.ProjectileDamage);
			DecreaseHP(playerDamage);
		}
	}
	public void OnDeathAnimationFinished()
	{
		QueueFree();
		//BUG: We never reach here if we spam shots and 'keep' the mob alive. We need to disable the hitbox once mob runs out of HP.
	}
	private void EnemyReadyAtAction()
	{
		EnemyReady = true;
	}
	private void ShootAtPlayer()
	{
		if (PlayerIsAlive())
		{
			EmitSignal(SignalName.SpawnNewEnemyProjectile, Position, util.NormalizeHalfStep(EnemyBaseDamage));
			EnemyProjectileCooldown.Start();
			EnemyReady = false;
		}
	}
	private void EnemyShootCooldownFinished()
	{
		EnemyReady = true;
	}

	private bool PlayerIsAlive()
	{
		return CoreManagerNode != null && CoreManagerNode.IsPlayerAlive();
	}
	private void InitiateCoreManager()
	{
		CoreManagerNode = GetParent().GetParent() as CoreManager;
		if (CoreManagerNode == null)
		{
			GD.PushError("Enemy: CoreManager parent is missing or wrong type.");
		}
	}
}
