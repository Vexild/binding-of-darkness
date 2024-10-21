using Godot;
using System;
using System.Security.Cryptography;

public partial class Enemy : Area2D
{
	public int EnemyMaxHP = 10;
	private int CurrentHp;
	[Signal]
	public delegate void OnHitByPlayerEventHandler();

	public override void _Ready()
	{
		GetNode<ProgressBar>("EnemyHP").Value = EnemyMaxHP;
		
		CurrentHp = EnemyMaxHP;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetNode<AnimatedSprite2D>("EnemyAnimation").Play("dummy");
	}

	public void DecreaseHP(int loss)
	{
		if (loss >= CurrentHp)
		{
			GD.Print("Death");
			GetNode<ProgressBar>("EnemyHP").Value = CurrentHp;
			GetNode<AnimatedSprite2D>("EnemyAnimation").Play("death_animation");
		}
		else
		{
			CurrentHp -= loss;
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
}
