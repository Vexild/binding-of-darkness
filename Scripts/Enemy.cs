using Godot;
using System;
using System.Security.Cryptography;

public partial class Enemy : Area2D
{
	[Export]
	public int EnemyMaxHP = 10;
	private int CurrentHp;
	[Signal]
	public delegate void OnHitByPlayerEventHandler();

	private AnimatedSprite2D enemyAnimation { get; set; }
	private ProgressBar hitPointBar { get; set; }
	public override void _Ready()
	{
		CurrentHp = EnemyMaxHP;
		hitPointBar = GetNode<ProgressBar>("EnemyHP");
		enemyAnimation = GetNode<AnimatedSprite2D>("EnemyAnimation");
		hitPointBar.Value = EnemyMaxHP;
		enemyAnimation.Play("idle");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void DecreaseHP(int loss)
	{
		CurrentHp -= loss;
		if (CurrentHp <= 0)
		{
			GetNode<ProgressBar>("EnemyHP").Value = CurrentHp;
			GD.Print("Death. Play the death animation");
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
	public void OnDeathAnimationFinished() {
		QueueFree();
	}
}
