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
	private ProgressBar HitPointBar { get; set; }
	private float CurrentHp;
	private AnimatedSprite2D enemyAnimation { get; set; }
	public CoreManager CoreManagerNode;
	private Vector2 PlayerLocation;
	public Util util = new Util();

	public override void _Ready()
	{
		InitiateCoreManager();
		InitiateEnemy();
	}

	public override void _Process(double delta)
	{
		
	}

	private void InitiateEnemy()
	{
		CurrentHp = EnemyMaxHP;
		HitPointBar = GetNode<ProgressBar>("EnemyHP");
		HitPointBar.Value = EnemyMaxHP;
		enemyAnimation = GetNode<AnimatedSprite2D>("EnemyAnimation");
		enemyAnimation.Play("idle");
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

	// public bool PlayerIsAlive()
	// {
	// 	return CoreManagerNode != null && CoreManagerNode.IsPlayerAlive();
	// }
	public void InitiateCoreManager()
	{
		CoreManagerNode = GetParent().GetParent() as CoreManager;
		if (CoreManagerNode == null)
		{
			GD.PushError("Enemy: CoreManager parent is missing or wrong type.");
		}
	}
}
