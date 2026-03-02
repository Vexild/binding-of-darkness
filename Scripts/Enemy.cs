using Godot;
using System;
using System.Security.Cryptography;

public partial class Enemy : Area2D
{

	[Export]
	public int EnemyMaxHP = 10;
	[Export]
	public int EnemyBaseDamage = 2;
	[Signal]
	public delegate void OnHitByPlayerEventHandler();

	[Signal]
	public delegate void SpawnNewEnemyProjectileEventHandler();

	private ProgressBar HitPointBar { get; set; }
	private int CurrentHp;
	private Timer EnemyProjectileCooldown;
	private bool EnemyReady = false;
	private AnimatedSprite2D enemyAnimation { get; set; }
	private Node RootNode;
	private Vector2 PlayerLocation;

	public override void _Ready()
	{
		RootNode = GetParent().GetParent();
		EnemyProjectileCooldown = GetNode<Timer>("EnemyShootCooldown");
		EnemyProjectileCooldown.Start();

		CurrentHp = EnemyMaxHP;
		HitPointBar = GetNode<ProgressBar>("EnemyHP");
		enemyAnimation = GetNode<AnimatedSprite2D>("EnemyAnimation");
		HitPointBar.Value = EnemyMaxHP;
		enemyAnimation.Play("idle");
	}

	public override void _Process(double delta)
	{
		if (EnemyReady)
		{
			ShootAtPlayer();
		}
	}

	public void DecreaseHP(int loss)
	{
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
			var projectileCast = (Projectile)proj;
			GD.Print(projectileCast.ProjectileDamage);
			DecreaseHP(projectileCast.ProjectileDamage);
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
		EmitSignal(SignalName.SpawnNewEnemyProjectile, Position, EnemyBaseDamage);
		EnemyProjectileCooldown.Start();
		EnemyReady = false;
	}
	private void EnemyShootCooldownFinished()
	{
		EnemyReady = true;
	}
}
