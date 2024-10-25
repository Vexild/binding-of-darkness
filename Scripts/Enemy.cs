using Godot;
using System;
using System.Security.Cryptography;

public partial class Enemy : Area2D
{

	[Export]
	public int EnemyMaxHP = 10;
	[Signal]
	public delegate void OnHitByPlayerEventHandler();

	[Signal]
	public delegate void ShootEnemyProjectileEventHandler(float pos, Vector2 dir);
	private ProgressBar HitPointBar { get; set; }
	private int CurrentHp;
	private Timer EnemyProjectileCooldown;
	private bool EnemyReady = false;
	private AnimatedSprite2D enemyAnimation { get; set; }
	private Node MainScene;
	private Vector2 PlayerLocation;

	public override void _Ready()
	{
		MainScene = GetParent().GetParent(); // Enemies are under node 'Enemies' which is under 'Main' scene
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
		PlayerLocation = MainScene.GetNode<CharacterBody2D>("Player").GetNode<Marker2D>("EnemyTargetPoint").GlobalPosition;
		if (EnemyReady)
		{
			ShootAtPlayer(PlayerLocation);
			EnemyReady = false;
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
		// We never reach here if we spam shots and 'keep' the mob alive. We need to disable the hitbox once mob runs out of HP.
	}
	private void EnemyReadyAtAction()
	{
		EnemyReady = true;
	}
	private void ShootAtPlayer(Vector2 PlayerPosition)
	{
		var dir = GlobalPosition.DirectionTo(PlayerPosition);
		EmitSignal(SignalName.ShootEnemyProjectile, Position, dir);
		EnemyProjectileCooldown.Start();
	}
	private void EnemyShootCooldownFinished()
	{
		EnemyReady = true;
	}
}
